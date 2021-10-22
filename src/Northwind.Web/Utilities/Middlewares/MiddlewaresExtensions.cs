using Microsoft.AspNetCore.Builder;

namespace Northwind.Web.Utilities.Middlewares
{
    public static class MiddlewaresExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
