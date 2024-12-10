using WDPR_2024.server.MyServerApp.Data;
using WDPR_2024.server.MyServerApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WDPR_2024.server.MyServerApp.DtoModels;

namespace WDPR_2024.server.MyServerApp.Services
{
    public class KlantService
    {
        private readonly AppDbContext _context;

        public KlantService(AppDbContext context)
        {
            _context = context;
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

        // Haal alle klanten op
        public async Task<List<Klant>> GetAlleKlantenAsync()
        {
            return await _context.Klanten
                .Include(k => k.Bedrijf)
                .ToListAsync();
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

            _context.Klanten.Remove(klant);
            await _context.SaveChangesAsync();
        }

        // Authenticeer een klant (voor login)
 public async Task<KlantDto> AuthenticateKlantAsync(KlantDto klantDto)
{
    // Retrieve the user by email
    var klant = await GetKlantByEmailAsync(klantDto.Email);
    if (klant == null || klantDto.Password != klant.Wachtwoord) // Plaintext comparison
    {
        throw new UnauthorizedAccessException("Invalid email or password.");
    }
    
    // Return only the safe data in KlantDto
    return new KlantDto
    {
        KlantID = klant.KlantID,
        Email = klant.Email
        
    };
}


    }
}
