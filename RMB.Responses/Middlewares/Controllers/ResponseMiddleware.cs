using Microsoft.AspNetCore.Http;
using Serilog;


namespace RMB.Responses.Middlewares.Controllers
{
    /// <summary>
    /// Middleware that modifies the HTTP response status code from 200 (OK) to 204 (No Content)
    /// if the response body is empty or contains only an empty JSON array ("[]").
    /// This helps improve API efficiency by reducing unnecessary response payloads.
    /// </summary>
    public class ResponseMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        public ResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Intercepts the HTTP response and modifies the status code to 204 (No Content) if applicable.
        /// This middleware ensures that responses do not include unnecessary empty content.
        /// </summary>
        /// <param name="context">The current HTTP request context.</param>
        public async Task Invoke(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            try
            {
                await _next(context);

                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                bool isEmptyJsonArray = responseBody.Trim() == "[]";

                if (context.Response.StatusCode == 200
                    && (string.IsNullOrWhiteSpace(responseBody) || isEmptyJsonArray)
                    && context.Response.ContentLength == null)
                {
                    Log.Warning("Alterando status de 200 (OK)  para 204 (NoContent)");
                    context.Response.StatusCode = StatusCodes.Status204NoContent;
                    context.Response.Body = new MemoryStream();  // Criando um novo MemoryStream vazio para evitar erro de escrita
                    return;
                }

                // Se o status NÃO for 204, restaurar a resposta original
                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;
            }
            catch (Exception ex)
            {
                context.Response.Body = originalBodyStream; // Restaura o stream original
                throw;
            }
        }
    }
}
