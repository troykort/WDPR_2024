﻿using Microsoft.Extensions.DependencyInjection;
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

            // Rollen die nodig zijn
            string[] roles = { "Backoffice", "Frontoffice", "Wagenparkbeheerder", "Abonnementbeheerder", "Klant" };

            // Creëer de rollen als ze nog niet bestaan
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation($"Role '{role}' created.");
                }
            }

            // Voeg een standaardgebruiker toe voor Backoffice
            var defaultUserEmail = "Dlamericaan@gmail.com";
            var defaultUser = await userManager.FindByEmailAsync(defaultUserEmail);
            if (defaultUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = defaultUserEmail,
                    Email = defaultUserEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin@123");
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

        // Functie om de rol van een gebruiker te wijzigen
        public static async Task UpdateUserRole(IServiceProvider serviceProvider, string userEmail, string newRole)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var logger = serviceProvider.GetRequiredService<ILogger<SeedData>>();

            // Controleer of de opgegeven rol bestaat
            var roleExists = await roleManager.RoleExistsAsync(newRole);
            if (!roleExists)
            {
                throw new Exception($"Rol '{newRole}' bestaat niet.");
            }

            // Vind de gebruiker op basis van e-mail
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new Exception("Gebruiker niet gevonden.");
            }

            // Verwijder de gebruiker uit alle rollen
            var currentRoles = await userManager.GetRolesAsync(user);
            foreach (var role in currentRoles)
            {
                await userManager.RemoveFromRoleAsync(user, role);
            }

            // Voeg de gebruiker toe aan de nieuwe rol
            await userManager.AddToRoleAsync(user, newRole);
            logger.LogInformation($"User '{userEmail}' role updated to '{newRole}'.");
        }
    }
}

