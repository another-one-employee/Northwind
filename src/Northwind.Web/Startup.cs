using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Northwind.Core;
using Northwind.Infrastructure;
using Northwind.Web.Utilities.Middlewares;
using System.Reflection;

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
            services.AddCoreServices();
            services.AddInfrastructureServices(Configuration);

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddControllersWithViews();
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
            app.UseMiddleware<ExceptionHandlerMiddleware>();

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
