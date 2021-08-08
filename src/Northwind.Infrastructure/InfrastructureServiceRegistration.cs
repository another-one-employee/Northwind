using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Application.Interfaces;
using Northwind.Application.Models;
using Northwind.Infrastructure.Data;
using Northwind.Infrastructure.Repositories;

namespace Northwind.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext<NorthwindDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString(nameof(NorthwindDbContext))))
                .AddDbContext<NorthwindIdentityDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString(nameof(NorthwindIdentityDbContext))));

            services
                .AddScoped<IAsyncRepository<Category>, EntityRepository<Category>>()
                .AddScoped<IAsyncRepository<Product>, ProductRepository>()
                .AddScoped<IAsyncRepository<Supplier>, EntityRepository<Supplier>>();

            services.AddScoped<DbContext, NorthwindDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<NorthwindIdentityDbContext>();

            return services;
        }
    }
}
