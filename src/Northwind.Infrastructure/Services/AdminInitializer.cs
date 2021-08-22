using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Northwind.Infrastructure.Services
{
    public static class AdminInitializer
    {
        public static async void InitializeAdminAsync(this IApplicationBuilder app, IConfiguration configuration)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            var adminSetting = configuration.GetSection(nameof(AdminInitializer));

            var email = adminSetting.GetSection("Email").Value;
            var password = adminSetting.GetSection("Password").Value;

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await userManager.FindByNameAsync(email) == null)
            {
                IdentityUser admin = new() { Email = email, UserName = email };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
