using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;


namespace RMB.Core.Middlewares
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex) 
            {
                await HandleValidationExceptionAsync(context, ex);
            }
        }

        private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var errors = new
            {
                Message = "Falha na Validação:",
                Errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            };

            var result = JsonSerializer.Serialize(errors);
            return context.Response.WriteAsync(result);
        }
    }
}
