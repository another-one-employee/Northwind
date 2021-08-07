using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using Northwind.Application.Exceptions;

namespace Northwind.Web.Utilities.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                HandleCustomExceptions(context, ex);
            }
        }

        private void HandleCustomExceptions(HttpContext context, Exception exception)
        {
            HttpStatusCode httpStatusCode = (HttpStatusCode)context.Response.StatusCode;

            switch (exception)
            {
                case NotFoundException:
                    httpStatusCode = HttpStatusCode.NotFound;
                    break;
            }
            
            context.Response.StatusCode = (int)httpStatusCode;
        }
    }
}
