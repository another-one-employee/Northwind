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
            switch (exception)
            {
                case NotFoundException:
                    var httpStatusCode = HttpStatusCode.NotFound;
                    context.Response.StatusCode = (int)httpStatusCode;
                    break;
            }
        }
    }
}
