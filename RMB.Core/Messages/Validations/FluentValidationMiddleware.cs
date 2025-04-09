using FluentValidation;
using Microsoft.Extensions.Logging;

namespace RMB.Core.Messages.Validations
{

    public class FluentValidationMiddleware<T> where T : class
    {
        private readonly IValidator<T> _validator;
        private readonly Func<T, CancellationToken, Task<bool>> _next;
        private readonly ILogger _logger;

        public FluentValidationMiddleware(
            IValidator<T> validator,
            Func<T, CancellationToken, Task<bool>> next,
            ILogger logger)
        {
            _validator = validator;
            _next = next;
            _logger = logger;
        }

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
