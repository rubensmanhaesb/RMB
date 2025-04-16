using FluentValidation;
using Newtonsoft.Json;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using RMB.Abstractions.Infrastructure.Messages.Exceptions;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;
using Serilog;
using System.Text;

namespace RMB.Core.Messages.Pipelines.Middlewares
{
    /// <summary>
    /// Middleware responsible for deserializing the raw message body into a strongly typed object.
    /// </summary>
    /// <typeparam name="T">The expected type of the deserialized message.</typeparam>
    /// <remarks>
    /// This middleware handles the JSON deserialization pipeline step, including:
    /// - UTF-8 decoding of the raw byte array
    /// - JSON deserialization using Newtonsoft.Json
    /// - Validation using FluentValidation
    /// - Success event publishing
    /// - Comprehensive error handling and logging
    /// </remarks>
    public class JsonDeserializationMiddleware<T> : IMessageMiddleware<T> where T : class
    {
        private readonly Func<T, CancellationToken, Task<bool>> _next;
        private readonly IValidator<T> _validator;
        private readonly IMessageSuccessEventPublisher _messageSuccessEventPublisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDeserializationMiddleware{T}"/> class.
        /// </summary>
        /// <param name="next">The next middleware delegate in the processing pipeline.</param>
        /// <param name="validator">The FluentValidation validator for type <typeparamref name="T"/>.</param>
        /// <param name="messageSuccessEventPublisher">The event publisher for successful deserializations (optional).</param>
        /// <remarks>
        /// Constructs the middleware with the next pipeline handler, validator, and optional success event publisher.
        /// </remarks>
        public JsonDeserializationMiddleware(
            Func<T, CancellationToken, Task<bool>> next,
            IValidator<T> validator,
            IMessageSuccessEventPublisher messageSuccessEventPublisher)
        {
            _next = next;
            _validator = validator;
            _messageSuccessEventPublisher = messageSuccessEventPublisher;
        }

         /// <summary>
        /// Processes the message by deserializing, validating, and passing to the next middleware.
        /// </summary>
        /// <param name="body">The raw message body as UTF-8 encoded bytes.</param>
        /// <param name="cancellationToken">The cancellation token for async operations.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a boolean indicating success.
        /// </returns>
        /// <exception cref="MessageDtoValidationException">
        /// Thrown when message validation fails.
        /// </exception>
        /// <exception cref="MessageDeserializationException">
        /// Thrown when JSON deserialization fails.
        /// </exception>
        /// <remarks>
        /// <para>
        /// The processing flow:
        /// 1. Decodes UTF-8 bytes to JSON string
        /// 2. Deserializes JSON to type <typeparamref name="T"/>
        /// 3. Validates the object using FluentValidation
        /// 4. Publishes success event if applicable
        /// 5. Invokes next middleware
        /// </para>
        /// <para>
        /// All exceptions are caught and wrapped in domain-specific exceptions with detailed context.
        /// </para>
        /// </remarks>
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

                _messageSuccessEventPublisher?.PublishSuccess(obj as EmailConfirmationMessage);

                return await _next(obj, cancellationToken);
            }
            catch (ValidationException vex)
            {
                throw CreateValidationException($"Erro na validação da mensagem JSON - {vex.Message}", json, vex, obj, typeof(T).Name);
            }
            catch (JsonException jsonEx)
            {
                throw CreateValidationException($"Erro ao desserializar a mensagem JSON - {jsonEx.Message}", json, jsonEx, obj, typeof(T).Name);
            }
            catch (Exception ex)
            {
                throw CreateValidationException($"Erro inesperado ao desserializar a mensagem JSON - {ex.Message}", json, ex, obj, typeof(T).Name);
            }
        }

        /// <summary>
        /// Creates a domain-specific exception with detailed error context.
        /// </summary>
        /// <param name="errorMessage">The primary error message.</param>
        /// <param name="json">The original JSON content that failed processing.</param>
        /// <param name="exception">The original exception (if any).</param>
        /// <param name="obj">The partially deserialized object (if available).</param>
        /// <param name="messageType">The name of the message type being processed.</param>
        /// <returns>
        /// A domain-specific exception containing all relevant error information.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Creates appropriate exception types based on the failure scenario:
        /// - <see cref="MessageDtoValidationException"/> for validation failures
        /// - <see cref="MessageDeserializationException"/> for deserialization failures
        /// </para>
        /// <para>
        /// Also logs the error and constructs a <see cref="MessageFailure"/> record for diagnostics.
        /// </para>
        /// </remarks>
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
                Id = emailConfirmationMessage?.Id ?? Guid.NewGuid(),
                SourceSystem = "RMB.CORE",
                FailureCategory = "Deserialize",
                FailureTimestamp = DateTime.UtcNow.AddTicks(-(DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond)),
                OriginalFailureMessage = exception.Message
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
