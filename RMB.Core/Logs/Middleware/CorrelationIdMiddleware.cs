using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using RMB.Abstractions.UseCases.Logs;

namespace RMB.Core.Logs.Middleware
{
    /// <summary>
    /// Middleware responsible for ensuring that each request has a unique Correlation ID.
    /// The Correlation ID is used for tracing requests across different services and logs.
    /// </summary>
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CorrelationIdMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the HTTP request pipeline.</param>
        /// <param name="correlationIdProvider">Service responsible for managing Correlation IDs.</param>
        public CorrelationIdMiddleware(RequestDelegate next, ICorrelationIdProvider correlationIdProvider)
        {
            _next = next;
            _correlationIdProvider = correlationIdProvider;
        }

        /// <summary>
        /// Middleware logic that assigns and logs the Correlation ID for each request.
        /// The Correlation ID is retrieved or generated and added to the response headers.
        /// </summary>
        /// <param name="context">The current HTTP request context.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = _correlationIdProvider.GetCorrelationId(context);

            context.Response.OnStarting(() =>
            {
                _correlationIdProvider.AddCorrelationIdToResponse(context);
                return Task.CompletedTask;
            });
            
            using (LogContext.PushProperty("CorrelationId", correlationId))
            try
            {
                Log.Information("Middleware do CorrelationId");
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error processing request: " );
                throw;
            }
        }
    }
}
