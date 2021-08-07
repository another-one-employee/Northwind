using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Northwind.Core.Interfaces;
using Northwind.Core.Services;

namespace Northwind.Core
{
    public static class CoreServiceRegistration
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}