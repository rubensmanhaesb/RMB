using Microsoft.AspNetCore.Http;
using Serilog;


namespace RMB.Responses.Middlewares.Controllers
{
    /// <summary>
    /// Middleware that changes the HTTP response status code from 200 (OK) to 204 (No Content)
    /// if the response body is empty or contains only an empty JSON array ("[]").
    /// </summary>
    public class ResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            try
            {
                await _next(context);

                // Voltar ao início para ler o corpo da resposta
                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                // Verifica se a resposta é uma lista vazia `[]`
                bool isEmptyJsonArray = responseBody.Trim() == "[]";

                // Se a resposta for 200 OK e vazia ou uma lista vazia, altera para 204
                if (context.Response.StatusCode == 200
                    && (string.IsNullOrWhiteSpace(responseBody) || isEmptyJsonArray)
                    && context.Response.ContentLength == null)
                {
                    Log.Logger.Information("Alterando status de 200 (OK)  para 204 (NoContent)");
                    context.Response.StatusCode = StatusCodes.Status204NoContent;
                    context.Response.Body.SetLength(0);
                    return;
                }

                // 🔹 Se o status NÃO for 204, restaurar a resposta original
                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;
            }
            catch (Exception ex)
            {
                // 🔹 Propaga o erro para que middlewares superiores o capturem
                context.Response.Body = originalBodyStream; // Restaura o stream original
                throw;
            }
        }
    }
}
