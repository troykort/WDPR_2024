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

            // Create a standard user for each role
            foreach (var role in roles)
            {
                var userEmail = $"{role.ToLower()}@example.com";
                var defaultPassword = $"{role}@123";
                var user = await userManager.FindByEmailAsync(userEmail);

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = userEmail,
                        Email = userEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, defaultPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                        logger.LogInformation($"User '{userEmail}' created with role '{role}' and password '{defaultPassword}'.");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            logger.LogError($"Error creating user '{userEmail}': {error.Description}");
                        }
                    }
                }
                else
                {
                    logger.LogInformation($"User '{userEmail}' already exists.");
                }
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
