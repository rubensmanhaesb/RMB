using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMB.Responses.Middlewares.Controllers
{
    public class ResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);
            if (context.Response.StatusCode == 200 && context.Response.ContentLength == null)
                 context.Response.StatusCode = StatusCodes.Status204NoContent; 
            
        }

    }
}
