using Microsoft.AspNetCore.Identity;
using TiendaEcomerce.Models;

namespace TiendaEcomerce.Extensions
{
    public static class IdentityDataInitializer
    {
        public static async Task SeedRolesAndAdminAsync(
            IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();
            string[] roles = new[] { "Admin", "Manager", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new ApplicationRole {Name=role});
            }
            // Admin user (lee credenciales desde config o secret manager)
            var adminEmail = configuration["Seed:AdminEmail"];
            var adminPassword = configuration["Seed:AdminPassword"];

            if (string.IsNullOrWhiteSpace(adminEmail) ||
                string.IsNullOrWhiteSpace(adminPassword))
                return; // no seed si no hay credenciales
            
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true, // o false si quieres confirmación por correo
                    FirstName = "System",
                    LastName = "Admin",
                    CreatedAt = DateTime.UtcNow
                };
                var result = await userManager
                    .CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    // loguear errores - preferible con ILogger
                    throw new Exception($"Error creando usuario admin:" +
                        $" {string.Join(',', result.Errors)}");
                }
            }
        }
    }
}
