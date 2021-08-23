using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Northwind.Application;
using Northwind.Infrastructure;
using Northwind.Infrastructure.Services;
using Northwind.Web.Utilities.Middlewares;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
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
            services
                .AddApplicationServices()
                .AddInfrastructureServices(Configuration);

            services
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddFluentValidation(s => 
                {
                    s.RegisterValidatorsFromAssemblyContaining<Startup>();
                });

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme);
            services.AddMicrosoftIdentityWebAppAuthentication(Configuration);

            services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme,
                options =>
                {
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                });

            services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddWebOptimizer();

            services.AddSwaggerGen(SetupSwaggerGen);
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            LoggingSetup(loggerFactory, Configuration);
            app.InitializeAdminAsync(Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(options => options.AllowAnyOrigin());
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "Northwind API V1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseWebOptimizer();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseImageCaching(Configuration);
            app.UseCustomExceptionHandler();

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

        private static void SetupSwaggerGen(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Northwind API",
                Contact = new OpenApiContact
                {
                    Name = "Srul1k",
                    Url = new Uri("https://t.me/Srul1k"),
                },
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        }
    }
}
