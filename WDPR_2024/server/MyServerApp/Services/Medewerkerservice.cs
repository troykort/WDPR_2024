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
            _emailService = emailService; // Initialiseer EmailService
        }

        // Haal alle medewerkers op
        public async Task<List<Medewerker>> GetAlleMedewerkersAsync()
        {
            return await _context.Medewerkers.ToListAsync();
        }

        // Voeg een nieuwe medewerker toe (geen nieuwe gebruiker aanmaken)
        public async Task AddMedewerkerAsync(Medewerker nieuweMedewerker, string rol)
        {
            // Controleer of de medewerker al bestaat op basis van e-mail
            if (await _context.Medewerkers.AnyAsync(m => m.Email == nieuweMedewerker.Email))
            {
                throw new Exception("Medewerker met dit e-mailadres bestaat al.");
            }

            // Sla de medewerker op in de Medewerkers tabel
            nieuweMedewerker.Rol = rol;
            _context.Medewerkers.Add(nieuweMedewerker);
            await _context.SaveChangesAsync();

            // Voeg de medewerker toe aan de rol
            var user = await _userManager.FindByEmailAsync(nieuweMedewerker.Email);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, rol);
            }

            // Verstuur welkomstmail naar de nieuwe medewerker
            var subject = "Welkom bij het team!";
            var body = $"Hallo {nieuweMedewerker.Naam},\n\nJe bent succesvol toegevoegd als {rol}.\nWelkom!";
            await _emailService.SendEmailAsync(nieuweMedewerker.Email, subject, body);
        }

        // Wijzig de rol van een medewerker
        public async Task UpdateMedewerkerRolAsync(int id, string nieuweRol)
        {
            var medewerker = await _context.Medewerkers.FindAsync(id);
            if (medewerker == null) throw new Exception("Medewerker niet gevonden.");

            var user = await _userManager.FindByEmailAsync(medewerker.Email);
            if (user == null) throw new Exception("Gebruiker gekoppeld aan medewerker niet gevonden.");

            // Controleer of de rol bestaat
            var roleExists = await _roleManager.RoleExistsAsync(nieuweRol);
            if (!roleExists)
            {
                throw new Exception($"De opgegeven rol '{nieuweRol}' bestaat niet.");
            }

            // Verwijder de medewerker uit de huidige rol en voeg de nieuwe rol toe
            var currentRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in currentRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            // Voeg de medewerker toe aan de nieuwe rol
            await _userManager.AddToRoleAsync(user, nieuweRol);

            // Werk de rol van de medewerker bij in de Medewerkers tabel
            medewerker.Rol = nieuweRol;
            await _context.SaveChangesAsync();

            // Verstuur e-mail naar de medewerker over de rolwijziging
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

            // Verstuur een e-mail naar de medewerker als ze zijn verwijderd (optioneel)
            var subject = "Je account is verwijderd";
            var body = $"Hallo {medewerker.Naam},\n\nJe account is verwijderd uit ons systeem.";
            await _emailService.SendEmailAsync(medewerker.Email, subject, body);
        }
    }
}