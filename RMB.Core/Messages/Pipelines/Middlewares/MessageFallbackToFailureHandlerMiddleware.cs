using Serilog;
using RMB.Abstractions.Infrastructure.Messages.Entities;
using RMB.Abstractions.Infrastructure.Messages.Exceptions;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;
using RMB.Core.Helpers;
using System.Text;
using FluentValidation;


namespace RMB.Core.Messages.Pipelines.Middlewares
{
    /// <summary>
    /// Middleware that wraps the pipeline and captures any failures (e.g., deserialization, validation).
    /// Delegates the failure to a dead-letter handler and event publisher.
    /// </summary>
    /// <typeparam name="T">The type of message being processed.</typeparam>
    public class MessageFallbackToFailureHandlerMiddleware<T> : IMessageMiddleware<T> where T : class
    {
        private readonly IMessageMiddleware<T> _next;
        private readonly IMessageDeadLetterHandler _failureHandler;
        private readonly IMessageBackgroundEventPublisher _eventPublisher;

        /// <summary>
        /// Initializes the middleware with the next pipeline step, a failure handler, and an event publisher.
        /// </summary>
        public MessageFallbackToFailureHandlerMiddleware(
            IMessageMiddleware<T> next,
            IMessageDeadLetterHandler failureHandler,
            IMessageBackgroundEventPublisher eventPublisher)
        {
            _next = next;
            _failureHandler = failureHandler;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Executes the next middleware and intercepts known exceptions.
        /// If an exception is thrown, delegates the error to the dead-letter handler and logs the failure.
        /// </summary>
        public async Task<bool> InvokeAsync(ReadOnlyMemory<byte> body, CancellationToken cancellationToken)
        {
            var bodyString = Encoding.UTF8.GetString(body.Span);
            string messageType = typeof(T).Name;

            try
            {
                await _next.InvokeAsync(body, cancellationToken);
                return true;
            }
            catch (ValidationException vex) 
            {
                await SendToDeadLetterAsync($"Erro de validação do DTO: {vex.Message}", vex, bodyString, messageType, "DtoValidationError", cancellationToken);
                return false;
            }
            catch (MessageDtoValidationException dtoEx)
            {
                await SendToDeadLetterAsync($"Erro de validação do DTO: {dtoEx.Message}", dtoEx, bodyString, messageType, "DtoValidationError", cancellationToken);
                return false;
            }
            catch (MessageDeserializationException desEx)
            {
                await SendToDeadLetterAsync($"Erro de desserialização: {desEx.Message}", desEx, bodyString, messageType, "DeserializationError", cancellationToken);
                return false;
            }
            catch (Exception ex)
            {
                await SendToDeadLetterAsync($"Erro inesperado ao processar a mensagem: {ex.Message}", ex, bodyString, messageType, "UnexpectedError", cancellationToken );
                throw;
            }
        }

        /// <summary>
        /// Sends the failed message to the dead-letter queue and triggers any background error event handlers.
        /// </summary>
        private async Task SendToDeadLetterAsync(string mensagemErro, Exception ex, string content, string messageType, string category, CancellationToken cancellationToken)
        {
            var messageFailure = new MessageFailure
                {
                    Id = Guid.NewGuid(),
                    SourceSystem = "RMB.Core",
                    FailureCategory = "DeadLetterHandler",
                    FailureTimestamp = DateTime.UtcNow
                };
            
            var tasks = new List<Task>
            {
                SafeExecutor.ExecuteSafeAsync(() =>
                {
                    Log.Error(ex, mensagemErro);
                }, "LogError"),

                SafeExecutor.ExecuteSafeAsync(() =>
                {
                    _eventPublisher?.PublishError(mensagemErro, ex);
                }, "PublishError"),

                SafeExecutor.ExecuteSafeAsync(() =>
                    _failureHandler.HandleAsync(content, mensagemErro, messageType, category, cancellationToken),
                    "DeadLetterHandler")
            };

            await Task.WhenAll(tasks);

            if (ex is MessagesException messagesEx)
            {
                messageFailure = messagesEx.MessageFailure;
            }

        }
    }
    
}