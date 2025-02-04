﻿using WDPR_2024.server.MyServerApp.Data;
using WDPR_2024.server.MyServerApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WDPR_2024.server.MyServerApp.Services
{
    public class MedewerkerService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EmailService _emailService;

        public MedewerkerService(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, EmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        // Haal alle medewerkers op
        public async Task<List<Medewerker>> GetAlleMedewerkersAsync()
        {
            return await _context.Medewerkers.ToListAsync();
        }

        // Voeg een nieuwe medewerker toe
        public async Task AddMedewerkerAsync(Medewerker nieuweMedewerker, string rol)
        {
            
            if (await _context.Medewerkers.AnyAsync(m => m.Email == nieuweMedewerker.Email))
            {
                throw new Exception("Medewerker met dit e-mailadres bestaat al.");
            }

           
            nieuweMedewerker.Rol = rol;
            _context.Medewerkers.Add(nieuweMedewerker);

          
            var user = new ApplicationUser
            {
                UserName = nieuweMedewerker.Email,
                Email = nieuweMedewerker.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, "Medewerker@123");
            if (!result.Succeeded)
            {
                throw new Exception("Fout bij het aanmaken van de medewerker.");
            }

            await _userManager.AddToRoleAsync(user, rol);

            
            var subject = "Welkom bij het team!";
            var body = $"Hallo {nieuweMedewerker.Naam},\n\nJe bent succesvol toegevoegd als {rol}.\nWelkom!";
            await _emailService.SendEmailAsync(nieuweMedewerker.Email, subject, body);

            await _context.SaveChangesAsync();
        }

     
        public async Task UpdateMedewerkerRolAsync(int id, string nieuweRol)
        {
            var medewerker = await _context.Medewerkers.FindAsync(id);
            if (medewerker == null) throw new Exception("Medewerker niet gevonden.");

            var user = await _userManager.FindByEmailAsync(medewerker.Email);
            if (user == null) throw new Exception("Gebruiker gekoppeld aan medewerker niet gevonden.");

          
            if (!await _roleManager.RoleExistsAsync(nieuweRol))
            {
                throw new Exception($"De opgegeven rol '{nieuweRol}' bestaat niet.");
            }

          
            var currentRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in currentRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            await _userManager.AddToRoleAsync(user, nieuweRol);

            
            medewerker.Rol = nieuweRol;
            await _context.SaveChangesAsync();

           
            var subject = "Je rol is gewijzigd!";
            var body = $"Hallo {medewerker.Naam},\n\nJe rol is succesvol gewijzigd naar {nieuweRol}.";
            await _emailService.SendEmailAsync(medewerker.Email, subject, body);
        }

        // Verwijder een medewerker
        public async Task DeleteMedewerkerAsync(int id)
        {
            var medewerker = await _context.Medewerkers.FindAsync(id);
            if (medewerker == null) throw new Exception("Medewerker niet gevonden.");

            var user = await _userManager.FindByEmailAsync(medewerker.Email);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            _context.Medewerkers.Remove(medewerker);
            await _context.SaveChangesAsync();

          
            var subject = "Je account is verwijderd";
            var body = $"Hallo {medewerker.Naam},\n\nJe account is verwijderd uit ons systeem.";
            await _emailService.SendEmailAsync(medewerker.Email, subject, body);
        }

        // Retrieve Medewerker by UserID
        public async Task<Medewerker?> GetMedewerkerByUserIdAsync(string userId)
        {
            return await _context.Medewerkers.FirstOrDefaultAsync(m => m.UserID == userId);
        }
        

    }
}
