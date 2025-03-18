using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Net;
using System.Text.Json;
using RMB.Abstractions.UseCases.Logs;

namespace RMB.Core.Logs.Middleware
{
    /// <summary>
    /// Middleware responsible for ensuring that every request has a unique Correlation ID.
    /// This ID is used for tracking and logging requests across different components.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly IHostEnvironment _hostEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="CorrelationIdMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        /// <param name="correlationIdProvider">Service responsible for managing Correlation ID generation and retrieval.</param>
        public ExceptionMiddleware(RequestDelegate next, ICorrelationIdProvider correlationIdProvider, IHostEnvironment hostEnvironment)
        {
            _next = next;
            _correlationIdProvider = correlationIdProvider;
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Middleware logic to ensure that each request has a Correlation ID.
        /// If a Correlation ID is not provided in the request headers, a new one is generated.
        /// The Correlation ID is then added to the logging context and response headers.
        /// </summary>
        /// <param name="context">The current HTTP request context.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Executa a requisição normalmente
            }
            catch (Exception ex)
            {
                var correlationId = _correlationIdProvider.GetCorrelationId(context);

                Log.Error(ex, "Ocorreu uma exceção não tratada:");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    status = context.Response.StatusCode,
                    message = _hostEnvironment.IsDevelopment() ? ex.Message : "Erro inesperado. Por favor, tente mais tarde.",
                    correlationId = correlationId
                };

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                if (_hostEnvironment.IsDevelopment())
                {
                    var devErrorResponse = new
                    {
                        status = errorResponse.status,
                        message = errorResponse.message,
                        correlationId = errorResponse.correlationId,
                        stackTrace = ex.StackTrace //  Adiciona stackTrace apenas no modo DEV
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(devErrorResponse, jsonOptions));
                }
                else
                {
                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, jsonOptions));
                }
            }
        }

    }
}
