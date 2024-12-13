using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WDPR_2024.server.MyServerApp.Models;

namespace WDPR_2024.server.MyServerApp.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var logger = serviceProvider.GetRequiredService<ILogger<SeedData>>();

            // Define roles
            string[] roles = { "Backoffice", "Frontoffice", "Wagenparkbeheerder", "Abonnementbeheerder", "Particulier", "Zakelijk" };

            // Ensure all roles exist
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation($"Role '{role}' created.");
                }
            }

            // Add default Backoffice user
            var defaultUserEmail = "backoffice@example.com";
            var defaultUser = await userManager.FindByEmailAsync(defaultUserEmail);
            if (defaultUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = defaultUserEmail,
                    Email = defaultUserEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Backoffice@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Backoffice");
                    logger.LogInformation($"User '{defaultUserEmail}' created and added to role 'Backoffice'.");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        logger.LogError($"Error creating user '{defaultUserEmail}': {error.Description}");
                    }
                }
            }
            else
            {
                logger.LogInformation($"User '{defaultUserEmail}' already exists.");
            }
        }

        // Update user role utility
        public static async Task UpdateUserRole(IServiceProvider serviceProvider, string userEmail, string newRole)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var logger = serviceProvider.GetRequiredService<ILogger<SeedData>>();

            if (!await roleManager.RoleExistsAsync(newRole))
            {
                throw new Exception($"Role '{newRole}' does not exist.");
            }

            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new Exception($"User with email '{userEmail}' not found.");
            }

            var currentRoles = await userManager.GetRolesAsync(user);
            foreach (var role in currentRoles)
            {
                await userManager.RemoveFromRoleAsync(user, role);
            }

            await userManager.AddToRoleAsync(user, newRole);
            logger.LogInformation($"User '{userEmail}' role updated to '{newRole}'.");
        }
    }
}
