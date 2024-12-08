using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WDPR_2024.server.MyServerApp.Data;
using WDPR_2024.server.MyServerApp.Models;

namespace WDPR_2024.server.MyServerApp.Services
{
    public class BedrijfService
    {
        private readonly AppDbContext _context;

        public BedrijfService(AppDbContext context)
        {
            _context = context;
        }

        // Haal een bedrijf op met medewerkers en abonnement
        public async Task<Bedrijf> GetBedrijfByIdAsync(int id)
        {
            return await _context.Bedrijven
                .Include(b => b.Medewerkers)
                .Include(b => b.Abonnement)
                .FirstOrDefaultAsync(b => b.BedrijfID == id);
        }

        // Haal alle bedrijven op
        public async Task<List<Bedrijf>> GetAlleBedrijvenAsync()
        {
            return await _context.Bedrijven
                .Include(b => b.Abonnement)
                .ToListAsync();
        }

        // Controleer of een KVK-nummer al bestaat
        public async Task<bool> IsKVKNummerUniekAsync(string kvkNummer)
        {
            return !await _context.Bedrijven.AnyAsync(b => b.KVKNummer == kvkNummer);
        }

        // Voeg een nieuw bedrijf toe
        public async Task AddBedrijfAsync(Bedrijf nieuwBedrijf)
        {
            if (!await IsKVKNummerUniekAsync(nieuwBedrijf.KVKNummer))
            {
                throw new Exception("Een bedrijf met dit KVK-nummer bestaat al.");
            }

            _context.Bedrijven.Add(nieuwBedrijf);
            await _context.SaveChangesAsync();
        }

        // Werk een bestaand bedrijf bij
        public async Task UpdateBedrijfAsync(int id, Bedrijf bijgewerktBedrijf)
        {
            var bestaandBedrijf = await _context.Bedrijven.FindAsync(id);
            if (bestaandBedrijf == null) throw new Exception("Bedrijf niet gevonden.");

            bestaandBedrijf.Bedrijfsnaam = bijgewerktBedrijf.Bedrijfsnaam;
            bestaandBedrijf.Adres = bijgewerktBedrijf.Adres;
            bestaandBedrijf.EmailDomein = bijgewerktBedrijf.EmailDomein;

            await _context.SaveChangesAsync();
        }

        // Verwijder een bedrijf
        public async Task DeleteBedrijfAsync(int id)
        {
            var bedrijf = await GetBedrijfByIdAsync(id);
            if (bedrijf == null) throw new Exception("Bedrijf niet gevonden.");

            _context.Bedrijven.Remove(bedrijf);
            await _context.SaveChangesAsync();
        }

        // Haal alle medewerkers van een bedrijf op
        public async Task<List<Klant>> GetMedewerkersVanBedrijfAsync(int bedrijfId)
        {
            return await _context.Klanten
                .Where(k => k.BedrijfID == bedrijfId)
                .ToListAsync();
        }
    }
}
