using Microsoft.AspNetCore.Http;
using RMB.Responses.Entities;
using System;
using System.Threading.Tasks;


namespace RMB.Responses.Middlewares.Logs
{

    public class CorrelationMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, CorrelationContext correlationContext)
        {
            // Se já vier um CorrelationId no cabeçalho, usa ele, senão gera um novo
            if (context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
            {
                correlationContext.CorrelationId = correlationId;
            }
            else
            {
                correlationContext.CorrelationId = Guid.NewGuid().ToString();
                context.Response.Headers.Add("X-Correlation-ID", correlationContext.CorrelationId);
            }

            await _next(context);
        }
    }

}
