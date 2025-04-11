using Microsoft.Extensions.Logging;
using RMB.Abstractions.Infrastructure.Messages.Interfaces;
using System.Text;

namespace RMB.Core.Messages.Pipelines
{
    /// <summary>
    /// Middleware that captures failed message pipeline executions and delegates to a fallback handler.
    /// </summary>
    public class FallbackToFailureHandlerMiddleware<T> : IMessageMiddleware<T> where T : class
    {
        private readonly IMessageMiddleware<T> _next;
        private readonly IDeadLetterHandler _failureHandler;
        private readonly ILogger _logger;

        public FallbackToFailureHandlerMiddleware(
            IMessageMiddleware<T> next,
            IDeadLetterHandler failureHandler,
            ILogger logger)
        {
            _next = next;
            _failureHandler = failureHandler;
            _logger = logger;
        }

        public async Task<bool> InvokeAsync(ReadOnlyMemory<byte> body, CancellationToken cancellationToken)
        {
            var bodyString = Encoding.UTF8.GetString(body.Span);

            try
            {
                var result = await _next.InvokeAsync(body, cancellationToken);
                if (!result)
                {
                    await _failureHandler.HandleAsync(
                        bodyString,
                        "Pipeline execution returned false.",
                        typeof(T).Name,
                        "ValidationError",
                        cancellationToken);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Pipeline execution failed.");

                await _failureHandler.HandleAsync(
                    bodyString,
                    ex.ToString(),
                    typeof(T).Name,
                    "DeserializationError",
                    cancellationToken);

                return false;
            }
        }
    }
}