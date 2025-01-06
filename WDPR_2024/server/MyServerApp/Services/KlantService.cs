using Microsoft.AspNetCore.Identity;
using WDPR_2024.server.MyServerApp.Data;
using WDPR_2024.server.MyServerApp.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WDPR_2024.server.MyServerApp.DtoModels;

namespace WDPR_2024.server.MyServerApp.Services
{
    public class KlantService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public KlantService(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Haal alle klanten op basis van emaildomein
        public async Task<List<Klant>> GetKlantenByEmailDomainAsync(string emailDomain)
        {
            return await _context.Klanten
                .Where(k => k.Email.EndsWith($"@{emailDomain}"))
                .ToListAsync();
        }

        // Haal een specifieke klant op
        public async Task<Klant> GetKlantByIdAsync(int id)
        {
            return await _context.Klanten
                .Include(k => k.Bedrijf)
                .FirstOrDefaultAsync(k => k.KlantID == id);
        }
        // Retrieve Klant by UserID
        public async Task<Klant?> GetKlantByUserIdAsync(string userId)
        {
            return await _context.Klanten.FirstOrDefaultAsync(k => k.UserID == userId);
        }


        // Haal alle klanten op
        public async Task<List<Klant>> GetAlleKlantenAsync()
        {
            return await _context.Klanten.Include(k => k.Bedrijf).ToListAsync();
        }

        // Controleer of een e-mailadres al is geregistreerd
        public async Task<bool> IsEmailGeregistreerdAsync(string email)
        {
            return await _context.Klanten.AnyAsync(k => k.Email == email);
        }

        // Haal een klant op via e-mailadres
        public async Task<Klant> GetKlantByEmailAsync(string email)
        {
            return await _context.Klanten.FirstOrDefaultAsync(k => k.Email == email);
        }

        // Verifieer het wachtwoord (placeholder: vervang door hashing in productie)
        public bool VerifyPassword(string inputPassword, string storedPassword)
        {
            // Voor een productieomgeving: gebruik hashing (bijv. BCrypt of een ander mechanisme)
            return inputPassword == storedPassword;
        }

        // Registreer een nieuwe klant
        public async Task AddKlantAsync(Klant nieuweKlant)
        {
            if (await IsEmailGeregistreerdAsync(nieuweKlant.Email))
            {
                throw new Exception("E-mailadres is al geregistreerd.");
            }

            // Automatische koppeling aan een bedrijf op basis van e-maildomein
            if (!string.IsNullOrEmpty(nieuweKlant.Email) && nieuweKlant.Email.Contains("@"))
            {
                var emailDomein = nieuweKlant.Email.Split('@')[1];
                var bedrijf = await _context.Bedrijven.FirstOrDefaultAsync(b => b.EmailDomein == emailDomein);
                if (bedrijf != null)
                {
                    nieuweKlant.BedrijfID = bedrijf.BedrijfID;
                }
            }

            // Maak een Identity-gebruiker
            var user = new ApplicationUser
            {
                UserName = nieuweKlant.Email,
                Email = nieuweKlant.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, nieuweKlant.Wachtwoord);
            if (!result.Succeeded)
            {
                throw new Exception("Fout bij het aanmaken van de gebruiker.");
            }

            // Wijs een standaardrol toe
            var role = nieuweKlant.BedrijfID.HasValue ? "Zakelijk" : "Particulier";
            await _userManager.AddToRoleAsync(user, role);

            // Koppel Identity-gebruiker aan Klant-model
            _context.Klanten.Add(nieuweKlant);
            await _context.SaveChangesAsync();
        }

        // Werk een klant bij
        public async Task UpdateKlantAsync(int id, Klant gewijzigdeKlant)
        {
            var bestaandeKlant = await _context.Klanten.FindAsync(id);
            if (bestaandeKlant == null) throw new Exception("Klant niet gevonden.");

            bestaandeKlant.Naam = gewijzigdeKlant.Naam;
            bestaandeKlant.Adres = gewijzigdeKlant.Adres;
            bestaandeKlant.Email = gewijzigdeKlant.Email;
            bestaandeKlant.Telefoonnummer = gewijzigdeKlant.Telefoonnummer;

            await _context.SaveChangesAsync();
        }

        // Verwijder een klant
        public async Task DeleteKlantAsync(int id)
        {
            var klant = await _context.Klanten.FindAsync(id);
            if (klant == null) throw new Exception("Klant niet gevonden.");

            var user = await _userManager.FindByEmailAsync(klant.Email);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            _context.Klanten.Remove(klant);
            await _context.SaveChangesAsync();
        }

    }
}
