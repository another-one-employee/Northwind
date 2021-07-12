using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Northwind.Core.Interfaces;
using Northwind.Core.Models;
using Northwind.Infrastructure.Data;
using Northwind.Infrastructure.Models;
using Northwind.Infrastructure.Repositories;
using Northwind.Web.Utilities.Middlewares;
using System;

namespace Northwind.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<NorthwindDbContext>(options =>
                options.UseSqlServer(connection));
            services.AddControllersWithViews();

            services.AddScoped<IRepository<CategoryDTO>, EntityRepository<Category, CategoryDTO>>();
            services.AddScoped<IRepository<ProductDTO>, ProductRepository>();
            services.AddScoped<IRepository<SupplierDTO>, EntityRepository<Supplier, SupplierDTO>>();

            services.AddScoped<DbContext, NorthwindDbContext>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            LoggingSetup(loggerFactory, Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<ImageCachingMiddleware>(Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        private static void LoggingSetup(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            loggerFactory.AddFile(configuration.GetSection("Logging"));

            var logger = loggerFactory.CreateLogger<Startup>();

            foreach (var config in configuration.GetChildren())
            {
                logger.LogInformation($"{config.Key,-35} : {config.Value}");
            }
        }
    }
}
