using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;

using RMB.Abstractions.UseCases.Logs;

namespace RMB.Core.Logs.Mediator.Behavior
{
    /// <summary>
    /// MediatR pipeline behavior that logs request execution details and ensures Correlation ID consistency.
    /// This behavior helps in tracking request flow and debugging by providing structured logging.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="correlationIdProvider">Service responsible for managing the Correlation ID.</param>
        /// <param name="httpContextAccessor">Provides access to the HTTP context.</param>
        public LoggingBehavior(ICorrelationIdProvider correlationIdProvider, IHttpContextAccessor httpContextAccessor)
        {
            _correlationIdProvider = correlationIdProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Logs request execution details, including the Correlation ID.
        /// Ensures that all logs contain a consistent Correlation ID for traceability.
        /// </summary>
        /// <param name="request">The incoming request object.</param>
        /// <param name="next">The delegate representing the next step in the pipeline.</param>
        /// <param name="cancellationToken">Token for cancelling the request processing.</param>
        /// <returns>The response from the next handler in the pipeline.</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            using (LogContext.PushProperty("Request", requestName))
            {
                Log.Information("Iniciando execução de {Request}", requestName);

                var response = await next(); 

                Log.Information("Execução de {Request} concluída", requestName);

                return response;
            }
        }
    }
}
