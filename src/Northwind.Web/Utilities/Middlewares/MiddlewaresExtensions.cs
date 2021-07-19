using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Northwind.Web.Utilities.Middlewares
{
    public static class MiddlewaresExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }

        public static IApplicationBuilder UseImageCaching(this IApplicationBuilder builder, IConfiguration configuration)
        {
            return builder.UseMiddleware<ImageCachingMiddleware>(configuration);
        }
    }
}
