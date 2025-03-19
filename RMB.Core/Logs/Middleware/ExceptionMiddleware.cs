using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Net;
using System.Text.Json;
using RMB.Abstractions.UseCases.Logs;

namespace RMB.Core.Logs.Middleware
{
    /// <summary>
    /// Middleware responsible for handling unhandled exceptions globally.
    /// It ensures that errors are logged and returns a consistent error response with a Correlation ID.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly IHostEnvironment _hostEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        /// <param name="correlationIdProvider">Service responsible for managing Correlation ID generation and retrieval.</param>
        /// <param name="hostEnvironment">Provides environment-specific information (e.g., Development or Production mode).</param>
        public ExceptionMiddleware(RequestDelegate next, ICorrelationIdProvider correlationIdProvider, IHostEnvironment hostEnvironment)
        {
            _next = next;
            _correlationIdProvider = correlationIdProvider;
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Middleware logic that captures unhandled exceptions, logs them, 
        /// and returns a structured error response with a Correlation ID.
        /// </summary>
        /// <param name="context">The current HTTP request context.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro inesperado: ");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    status = context.Response.StatusCode,
                    message = _hostEnvironment.IsDevelopment() ? ex.Message : "Erro inesperado. Por favor, tente mais tarde."
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
