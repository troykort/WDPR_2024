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
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
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

            
            var users = new[]
            {
    new
    {
        Email = "backoffice@example.com",
        Password = "Backoffice@123",
        Role = "Backoffice",
        Name = "Back Office Admin",
        Address = "Backoffice Street 1",
        Phone = "010-1234567",
        Rijbewijs = (string)null
    },
    new
    {
        Email = "frontoffice@example.com",
        Password = "Frontoffice@123",
        Role = "Frontoffice",
        Name = "Front Office Admin",
        Address = "Frontoffice Avenue 2",
        Phone = "010-2345678",
        Rijbewijs = (string)null
    },
    new
    {
        Email = "wagenparkbeheerder@example.com",
        Password = "Wagenpark@123",
        Role = "Wagenparkbeheerder",
        Name = "Fleet Manager",
        Address = "Wagenpark Way 3",
        Phone = "010-3456789",
        Rijbewijs = (string)null
    },
    new
    {
        Email = "abonnementbeheerder@example.com",
        Password = "Abonnement@123",
        Role = "Abonnementbeheerder",
        Name = "Subscription Admin",
        Address = "Abonnement Alley 4",
        Phone = "010-4567890",
        Rijbewijs = (string)null
    },
    new
    {
        Email = "particulier@example.com",
        Password = "Particulier@123",
        Role = "Particulier",
        Name = "John Doe",
        Address = "Particulier Lane 5",
        Phone = "010-5678901",
        Rijbewijs = "AB123456"
    },
    new
    {
        Email = "zakelijk@example.com",
        Password = "Zakelijk@123",
        Role = "Zakelijk",
        Name = "Jane Smith",
        Address = "Zakelijk Road 6",
        Phone = "010-6789012",
        Rijbewijs = (string)null
    }
};


            // Add or update users
            foreach (var userInfo in users)
            {
                var user = await userManager.FindByEmailAsync(userInfo.Email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = userInfo.Email,
                        Email = userInfo.Email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, userInfo.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, userInfo.Role);

                        // Assign user to the corresponding model
                        if (userInfo.Role == "Particulier" || userInfo.Role == "Zakelijk")
                        {
                            var klant = new Klant
                            {
                                UserID = user.Id,
                                Naam = userInfo.Name,
                                Adres = userInfo.Address,
                                Email = userInfo.Email,
                                Wachtwoord = userInfo.Password,
                                Telefoonnummer = userInfo.Phone,
                                RijbewijsNummer = userInfo.Rijbewijs,
                                IsActief = true
                            };
                            dbContext.Klanten.Add(klant);
                        }
                        else
                        {
                            var medewerker = new Medewerker
                            {
                                UserID = user.Id,
                                Naam = userInfo.Name,
                                Email = userInfo.Email,
                                Rol = userInfo.Role
                            };
                            dbContext.Medewerkers.Add(medewerker);
                        }

                        logger.LogInformation($"User '{userInfo.Email}' created and added to role '{userInfo.Role}'.");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            logger.LogError($"Error creating user '{userInfo.Email}': {error.Description}");
                        }
                    }
                }
                else
                {
                    logger.LogInformation($"User '{userInfo.Email}' already exists.");
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
