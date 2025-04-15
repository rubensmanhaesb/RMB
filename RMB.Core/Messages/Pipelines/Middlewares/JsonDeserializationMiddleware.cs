using FluentValidation;
using Newtonsoft.Json;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using RMB.Abstractions.Infrastructure.Messages.Exceptions;
using Serilog;
using System.Text;

namespace RMB.Core.Messages.Pipelines.Middlewares
{
    /// <summary>
    /// Middleware responsible for deserializing the raw message body into a strongly typed object.
    /// </summary>
    /// <typeparam name="T">The expected type of the deserialized message.</typeparam>
    public class JsonDeserializationMiddleware<T> : IMessageMiddleware<T> where T : class
    {
        private readonly Func<T, CancellationToken, Task<bool>> _next;
        private readonly IValidator<T> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDeserializationMiddleware{T}"/> class.
        /// </summary>
        /// <param name="next">The next middleware to invoke after successful deserialization and validation.</param>
        /// <param name="validator">The validator for the deserialized object.</param>
        public JsonDeserializationMiddleware(
            Func<T, CancellationToken, Task<bool>> next,
            IValidator<T> validator)
        {
            _next = next;
            _validator = validator;
        }

        /// <summary>
        /// Attempts to deserialize the message from JSON and validate it using FluentValidation.
        /// If successful, the pipeline continues with the next middleware.
        /// </summary>
        /// <param name="body">The raw message body.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns><c>true</c> if the processing succeeded; otherwise, an exception is thrown.</returns>
        public async Task<bool> InvokeAsync(ReadOnlyMemory<byte> body, CancellationToken cancellationToken)
        {
            string json = string.Empty;
            T obj = null;

            try
            {
                json = Encoding.UTF8.GetString(body.Span);
                obj = JsonConvert.DeserializeObject<T>(json);

                if (obj == null)
                {
                    CreateValidationException($"Falha na desserialização do JSON igual null {json}", json, null, obj, typeof(T).Name);
                }

                await _validator.ValidateAndThrowAsync(obj);

                return await _next(obj, cancellationToken);
            }
            catch (ValidationException vex)
            {
                throw CreateValidationException($"Erro na validação da mensagem JSON.", json, vex, obj, typeof(T).Name);
            }
            catch (JsonException jsonEx)
            {
                throw CreateValidationException($"Erro ao desserializar a mensagem JSON.", json, jsonEx, obj, typeof(T).Name);
            }
            catch (Exception ex)
            {
                throw CreateValidationException($"Erro inesperado ao desserializar a mensagem JSON.", json, ex, obj, typeof(T).Name);
            }
        }

        /// <summary>
        /// Creates a domain-specific exception object based on the type of error encountered.
        /// </summary>
        /// <param name="mensagemErro">The error message to log and wrap.</param>
        /// <param name="json">The original JSON content that failed processing.</param>
        /// <param name="exception">The original exception.</param>
        /// <param name="obj">The deserialized object, if available.</param>
        /// <param name="messageType">The message type name.</param>
        /// <returns>A specific exception instance related to the deserialization or validation error.</returns>
        private Exception CreateValidationException(
            string mensagemErro,
            string json,
            Exception exception,
            T obj,
            string messageType)
        {
            var emailConfirmationMessage = obj as EmailConfirmationMessage;

            var messageFailure = new MessageFailure
            {
                Id = emailConfirmationMessage?.UserId ?? Guid.NewGuid(),
                SourceSystem = "RMB.CORE",
                FailureCategory = "Deserialize",
                FailureTimestamp = DateTime.UtcNow.AddTicks(-(DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond))
            };

            Log.Warning(mensagemErro);

            return exception switch
            {
                ValidationException => new MessageDtoValidationException(
                    mensagemErro, json, emailConfirmationMessage, messageFailure, exception),

                _ => new MessageDeserializationException(
                    mensagemErro, json, exception)
            };
        }
    }

}
