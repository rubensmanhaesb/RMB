using FluentValidation;
using Microsoft.Extensions.Logging;

namespace RMB.Core.Messages.Validations
{
    /// <summary>
    /// Middleware that validates a deserialized message using FluentValidation before passing it to the next stage in the pipeline.
    /// </summary>
    /// <typeparam name="T">The type of message to validate.</typeparam>
    public class FluentValidationMiddleware<T> where T : class
    {
        private readonly IValidator<T> _validator;
        private readonly Func<T, CancellationToken, Task<bool>> _next;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes the middleware with a validator, the next middleware, and a logger.
        /// </summary>
        /// <param name="validator">Validator for the message type.</param>
        /// <param name="next">Next middleware in the pipeline.</param>
        /// <param name="logger">Logger for outputting validation failures.</param>
        public FluentValidationMiddleware(
            IValidator<T> validator,
            Func<T, CancellationToken, Task<bool>> next,
            ILogger logger)
        {
            _validator = validator;
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Executes the validation logic. If the message is valid, proceeds to the next middleware.
        /// </summary>
        /// <param name="message">The message to validate.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>True if the message passes validation and continues, otherwise false.</returns>
        public async Task<bool> InvokeAsync(T message, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(message, cancellationToken);
            if (!result.IsValid)
            {
                _logger.LogWarning("Message failed validation: {errors}", string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
                return false;
            }

            return await _next(message, cancellationToken);
        }
    }

}
