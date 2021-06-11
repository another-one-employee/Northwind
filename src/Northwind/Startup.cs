using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Northwind.Models;

namespace Northwind
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
            services.AddDbContext<NorthwindDataContext>(options =>
                options.UseSqlServer(connection));
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void LoggingSetup(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            loggerFactory.AddFile(configuration.GetSection("Logging"));

            var logger = loggerFactory.CreateLogger<Startup>();

            foreach (var config in configuration.GetChildren())
            {
                logger.LogInformation($"{config.Key, -35} : {config.Value}");
            }
        }
    }
}
