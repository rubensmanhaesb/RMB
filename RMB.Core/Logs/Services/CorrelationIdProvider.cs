using Microsoft.AspNetCore.Http;
using RMB.Abstractions.UseCases.Logs;
using RMB.Core.Logs.Helpers;


namespace RMB.Core.Logs.Services
{
    public class CorrelationIdProvider : ICorrelationIdProvider
    {
        private const string CorrelationHeader = "X-Correlation-ID"; // Standard correlation ID header name


        public string GetCorrelationId(HttpContext context)
        {
            if (context == null)
                return CorrelationIdGenerator.GenerateNewCorrelationId();

            if (context.Items.ContainsKey(CorrelationHeader))
                return context.Items[CorrelationHeader]?.ToString() ?? CorrelationIdGenerator.GenerateNewCorrelationId();

            var correlationId = context.Request.Headers[CorrelationHeader].FirstOrDefault() ?? CorrelationIdGenerator.GenerateNewCorrelationId();

            context.Items[CorrelationHeader] = correlationId;
            return correlationId;
        }

        public void AddCorrelationIdToResponse(HttpContext context)
        {
            if (context == null)
                return;

            var correlationId = GetCorrelationId(context);

            if (!context.Response.Headers.ContainsKey(CorrelationHeader))
                context.Response.Headers[CorrelationHeader] = correlationId;
        }
    }
}
