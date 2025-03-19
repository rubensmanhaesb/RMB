using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RMB.Core.Logs.Settings;
using Serilog;

namespace RMB.Core.Logs.Middleware
{
    /// <summary>
    /// Middleware responsible for logging incoming HTTP requests and outgoing responses.
    /// It supports dynamic configuration updates for enabling or disabling logging via app settings.
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptionsMonitor<LoggingSettings> _loggingSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestResponseLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        /// <param name="loggingSettings">Provides dynamic access to logging configuration settings.</param>
        public RequestResponseLoggingMiddleware(RequestDelegate next, IOptionsMonitor<LoggingSettings> loggingSettings)
        {
            _next = next;
            _loggingSettings = loggingSettings;

            _loggingSettings.OnChange(settings =>
            {
                Log.Warning("Configuração de logging atualizada: LogRequestsAndResponses = {Value}", settings.LogRequestsAndResponses);
            });
        }

        /// <summary>
        /// Intercepts and logs HTTP request and response details, if logging is enabled.
        /// </summary>
        /// <param name="context">The current HTTP request context.</param>
        public async Task Invoke(HttpContext context)
        {
            if (!_loggingSettings.CurrentValue.LogRequestsAndResponses)
            {
                await _next(context);
                return;
            }

            var requestBody = await ReadRequestBody(context);
            Log.Information("Request: {Method} {Path} | Body: {Body}",
                context.Request.Method, context.Request.Path, requestBody);

            // Substituir o Response.Body temporariamente para capturar a resposta
            var originalResponseBodyStream = context.Response.Body;
            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context); // Executar o próximo middleware

                responseBody.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();
                responseBody.Seek(0, SeekOrigin.Begin);

                Log.Information("Response: {StatusCode} | Body: {Body}",
                    context.Response.StatusCode, responseBodyText);

                // Restaurar o Response.Body original antes de passar para o próximo middleware
                await responseBody.CopyToAsync(originalResponseBodyStream);
            }
            catch (Exception ex)
            {
                Log.Warning("Response: {StatusCode} | Body: {Body}",
                    context.Response.StatusCode, "Verifique o detalhamento do erro no próximo registro de log.");

                throw;
            }
            finally
            {
                // Restaurar o fluxo original no finally para evitar que seja fechado prematuramente
                context.Response.Body = originalResponseBodyStream;
                responseBody.Dispose();
            }
        }

        /// <summary>
        /// Reads the HTTP request body as a string while preserving the stream position.
        /// </summary>
        /// <param name="context">The HTTP context containing the request.</param>
        /// <returns>The request body as a string, truncated if it exceeds 5000 characters.</returns>
        private async Task<string> ReadRequestBody(HttpContext context)
        {
            var request = context.Request;

            if (request.ContentLength == null || request.ContentLength == 0)
                return string.Empty;

            request.EnableBuffering();

            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;

            return body.Length > 5000 ? body.Substring(0, 5000) + "..." : body;
        }
    }
}
