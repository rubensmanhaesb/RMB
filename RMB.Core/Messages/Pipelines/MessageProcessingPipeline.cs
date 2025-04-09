using FluentValidation;
using Microsoft.Extensions.Logging;
using RMB.Core.Messages.Validations;


namespace RMB.Core.Messages.Pipelines
{
    /// <summary>
    /// Provides a reusable message processing pipeline composed of middleware components
    /// like JSON deserialization and FluentValidation.
    /// </summary>
    public static class MessageProcessingPipeline
    {
        /// <summary>
        /// Builds a message processing pipeline with deserialization, validation, and a final handler.
        /// </summary>
        /// <typeparam name="T">The type of the message to process.</typeparam>
        /// <param name="validator">The validator used to validate the deserialized message.</param>
        /// <param name="finalHandler">The final handler to execute after successful validation.</param>
        /// <param name="logger">Logger instance for logging errors and info.</param>
        /// <returns>A composed <see cref="IMessageMiddleware{T}"/> pipeline ready to process messages.</returns>
        public static IMessageMiddleware<T> Build<T>(
            IValidator<T> validator,
            Func<T, CancellationToken, Task> finalHandler,
            ILogger logger)
            where T : class
        {
            // Middleware to validate the deserialized message
            var validationMiddleware = new FluentValidationMiddleware<T>(validator, async (msg, ct) =>
            {
                await finalHandler(msg, ct);
                return true;
            }, logger);

            // Middleware to deserialize the message from JSON before passing to validation
            return new JsonDeserializationMiddleware<T>(
                validationMiddleware.InvokeAsync,
                logger
            );
        }
    }

}
