using Serilog;
using RMB.Abstractions.Infrastructure.Messages.Exceptions;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;
using RMB.Core.Helpers;
using System.Text;
using FluentValidation;


namespace RMB.Core.Messages.Pipelines.Middlewares
{
    /// <summary>
    /// Middleware that provides error handling and dead-letter queue functionality for message processing pipelines.
    /// </summary>
    /// <typeparam name="T">The type of message being processed.</typeparam>
    /// <remarks>
    /// This middleware serves as a safety net in the message processing pipeline by:
    /// 1. Intercepting and classifying different types of failures
    /// 2. Publishing error events for monitoring
    /// 3. Ensuring failed messages are properly archived in a dead-letter queue
    /// 4. Providing detailed logging for troubleshooting
    /// </remarks>
    public class MessageFallbackToFailureHandlerMiddleware<T> : IMessageMiddleware<T> where T : class
    {
        private readonly IMessageMiddleware<T> _next;
        private readonly IMessageDeadLetterHandler _messageDeadLetterHandler;
        private readonly IMessageErrorEventPublisher _messageBackgroundEventPublisher;

        /// <summary>
        /// Initializes a new instance of the error handling middleware.
        /// </summary>
        /// <param name="next">The next middleware in the processing pipeline.</param>
        /// <param name="messageDeadLetterHandler">The dead-letter queue handler for failed messages.</param>
        /// <param name="messageBackgroundEventPublisher">The error event publisher (optional).</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if next or failureHandler parameters are null.
        /// </exception>
        public MessageFallbackToFailureHandlerMiddleware(
            IMessageMiddleware<T> next,
            IMessageDeadLetterHandler messageDeadLetterHandler,
            IMessageErrorEventPublisher messageBackgroundEventPublisher)
        {
            _next = next;
            _messageDeadLetterHandler = messageDeadLetterHandler;
            _messageBackgroundEventPublisher = messageBackgroundEventPublisher;
        }

        /// <summary>
        /// Executes the message processing pipeline with comprehensive error handling.
        /// </summary>
        /// <param name="body">The raw message content as bytes.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// Returns true if processing succeeded, false if the message was handled as a dead-letter.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The method handles several specific exception types differently:
        /// - ValidationException: Business rule violations
        /// - MessageDtoValidationException: DTO structural issues
        /// - MessageDeserializationException: Message format problems
        /// - Other exceptions: Considered system-level failures
        /// </para>
        /// <para>
        /// Unexpected exceptions are rethrown to bubble up to higher-level handlers.
        /// </para>
        /// </remarks>
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
        /// Handles the failed message by executing all failure handling tasks in parallel.
        /// </summary>
        /// <param name="errorMessage">Descriptive error message.</param>
        /// <param name="ex">The exception that caused the failure.</param>
        /// <param name="content">The original message content.</param>
        /// <param name="messageType">The type name of the message.</param>
        /// <param name="category">The error category for classification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// <para>
        /// Executes three key operations in parallel:
        /// 1. Error logging (Serilog)
        /// 2. Error event publishing
        /// 3. Dead-letter queue submission
        /// </para>
        /// <para>
        /// Each operation is wrapped in SafeExecutor to prevent one failure from affecting others.
        /// </para>
        /// </remarks>
        private async Task SendToDeadLetterAsync(string mensagemErro, Exception ex, string content, string messageType, string category, CancellationToken cancellationToken)
        {

            var tasks = new List<Task>
            {
                SafeExecutor.ExecuteSafeAsync(() =>
                {
                    Log.Error(ex, mensagemErro);
                }, "LogError"),

                SafeExecutor.ExecuteSafeAsync(() =>
                {
                    _messageBackgroundEventPublisher?.PublishError(mensagemErro, ex);
                }, "PublishError"),

                SafeExecutor.ExecuteSafeAsync(() =>
                    _messageDeadLetterHandler.HandleAsync(content, mensagemErro, messageType, category, cancellationToken),
                    "DeadLetterHandler")
            };

            await Task.WhenAll(tasks);

        }
    }
    
}