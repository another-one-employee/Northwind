using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Core.Interfaces;
using Northwind.Core.Models;
using Northwind.Infrastructure.Data;
using Northwind.Infrastructure.Repositories;

namespace Northwind.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connection = configuration.GetConnectionString("NorthwindConnectionString");
            services.AddDbContext<NorthwindDbContext>(options =>
                options.UseSqlServer(connection));

            services.AddScoped<IAsyncRepository<Category>, EntityRepository<Category>>();
            services.AddScoped<IAsyncRepository<Product>, ProductRepository>();
            services.AddScoped<IAsyncRepository<Supplier>, EntityRepository<Supplier>>();

            services.AddScoped<DbContext, NorthwindDbContext>();

            return services;
        }
    }
}
