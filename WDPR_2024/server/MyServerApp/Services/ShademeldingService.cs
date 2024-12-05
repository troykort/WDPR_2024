using WDPR_2024.server.MyServerApp.Data;
using WDPR_2024.server.MyServerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace WDPR_2024.server.MyServerApp.Services
{
    public class SchademeldingService
    {
        private readonly AppDbContext _context;

        public SchademeldingService(AppDbContext context)
        {
            _context = context;
        }

        // Haal een specifieke schademelding op
        public async Task<Schademelding> GetSchademeldingByIdAsync(int id)
        {
            return await _context.Schademeldingen
                .Include(s => s.Voertuig)
                .Include(s => s.Klant)
                .FirstOrDefaultAsync(s => s.SchademeldingID == id);
        }

        // Haal alle schademeldingen op
        public async Task<List<Schademelding>> GetAlleSchademeldingenAsync()
        {
            return await _context.Schademeldingen
                .Include(s => s.Voertuig)
                .Include(s => s.Klant)
                .ToListAsync();
        }

        // Voeg een nieuwe schademelding toe
        public async Task AddSchademeldingAsync(Schademelding nieuweSchademelding)
        {
            _context.Schademeldingen.Add(nieuweSchademelding);
            await _context.SaveChangesAsync();
        }

        // Werk de status van een schademelding bij
        public async Task UpdateSchademeldingStatusAsync(int id, string nieuweStatus, string opmerkingen = null)
        {
            var schademelding = await _context.Schademeldingen.FindAsync(id);
            if (schademelding == null) throw new Exception("Schademelding niet gevonden.");

            schademelding.Status = nieuweStatus;
            schademelding.Opmerkingen = opmerkingen;

            await _context.SaveChangesAsync();
        }

        // Verwijder een schademelding
        public async Task DeleteSchademeldingAsync(int id)
        {
            var schademelding = await _context.Schademeldingen.FindAsync(id);
            if (schademelding == null) throw new Exception("Schademelding niet gevonden.");

            _context.Schademeldingen.Remove(schademelding);
            await _context.SaveChangesAsync();
        }
    }
}
