using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Infrastructure.Annotations;

namespace Northwind.Infrastructure.Services
{
    public static class AdminInitializer
    {
        private static string emailSection = "Email";
        private static string passwordSection = "Password";

        public static async void InitializeAdminAsync(this IApplicationBuilder app, IConfiguration configuration)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            var adminSetting = configuration.GetSection(nameof(AdminInitializer));

            var email = adminSetting.GetSection(emailSection).Value;
            var password = adminSetting.GetSection(passwordSection).Value;

            if (roleManager != null && await roleManager.FindByNameAsync(nameof(Roles.admin)) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(nameof(Roles.admin)));
            }

            if (userManager != null && await userManager.FindByNameAsync(email) == null)
            {
                IdentityUser admin = new() { Email = email, UserName = email };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, nameof(Roles.admin));
                }
            }
        }
    }
}
