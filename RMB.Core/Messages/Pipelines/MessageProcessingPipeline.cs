using FluentValidation;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;
using RMB.Core.Messages.Pipelines.Middlewares;

namespace RMB.Core.Messages.Pipelines
{
    /// <summary>
    /// Factory for creating standardized message processing pipelines with middleware components.
    /// </summary>
    /// <remarks>
    /// Provides a pre-configured pipeline that handles:
    /// - Message deserialization
    /// - Validation
    /// - Error handling
    /// - Success/failure event publishing
    /// - Dead-letter queue routing
    /// </remarks>
    public static class MessageProcessingPipeline
    {
        /// <summary>
        /// Constructs a complete message processing pipeline with error handling and validation.
        /// </summary>
        /// <typeparam name="T">The message type to be processed.</typeparam>
        /// <param name="validator">FluentValidation validator for message validation.</param>
        /// <param name="finalHandler">Business logic handler for valid messages.</param>
        /// <param name="failureHandler">Handler for failed message processing.</param>
        /// <param name="eventPublisher">Publisher for error events.</param>
        /// <param name="messageSuccessEventPublisher">Publisher for success events (optional).</param>
        /// <returns>A configured message processing pipeline.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if validator, finalHandler or failureHandler is null.
        /// </exception>
        /// <example>
        /// <code>
        /// var pipeline = MessageProcessingPipeline.Build(
        ///     validator: new MyMessageValidator(),
        ///     finalHandler: ProcessMessageAsync,
        ///     failureHandler: deadLetterHandler,
        ///     eventPublisher: errorPublisher,
        ///     messageSuccessEventPublisher: successPublisher
        /// );
        /// </code>
        /// </example>
        public static IMessageMiddleware<T> Build<T>(
        IValidator<T> validator,
            Func<T, CancellationToken, Task> finalHandler,
            IMessageDeadLetterHandler failureHandler,
            IMessageErrorEventPublisher eventPublisher,
            IMessageSuccessEventPublisher messageSuccessEventPublisher)
            where T : class
        {

            var deserializationMiddleware = new JsonDeserializationMiddleware<T>(
                async (msg, ct) =>
                 {
                     await finalHandler(msg, ct);
                     return true;
                 },
                validator,
                messageSuccessEventPublisher
            );

            return new MessageFallbackToFailureHandlerMiddleware<T>(
                deserializationMiddleware,
                failureHandler,
                eventPublisher
            );
        }
    }

}
