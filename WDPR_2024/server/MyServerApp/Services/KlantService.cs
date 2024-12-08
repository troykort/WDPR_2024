using WDPR_2024.server.MyServerApp.Data;
using WDPR_2024.server.MyServerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace WDPR_2024.server.MyServerApp.Services
{
    public class KlantService
    {
        private readonly AppDbContext _context;

        public KlantService(AppDbContext context)
        {
            _context = context;
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
    }
}
