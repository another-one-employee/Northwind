using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Core.Interfaces;
using Northwind.Core.Models;
using Northwind.Infrastructure.Data;
using Northwind.Infrastructure.Models;
using Northwind.Infrastructure.Repositories;
using System.Reflection;

namespace Northwind.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connection = configuration.GetConnectionString("NorthwindConnectionString");
            services.AddDbContext<NorthwindDbContext>(options =>
                options.UseSqlServer(connection));

            services.AddScoped<IAsyncRepository<CategoryDTO>, EntityRepository<Category, CategoryDTO>>();
            services.AddScoped<IAsyncRepository<ProductDTO>, ProductRepository>();
            services.AddScoped<IAsyncRepository<SupplierDTO>, EntityRepository<Supplier, SupplierDTO>>();

            services.AddScoped<DbContext, NorthwindDbContext>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
