using WDPR_2024.server.MyServerApp.Data;
using WDPR_2024.server.MyServerApp.Models;
using Microsoft.EntityFrameworkCore;
namespace WDPR_2024.server.MyServerApp.Services
{
    public class AbonnementService
    {
        private readonly AppDbContext _context;

        public AbonnementService(AppDbContext context)
        {
            _context = context;
        }

        // Haal een specifiek abonnement op
        public async Task<Abonnement> GetAbonnementByIdAsync(int id)
        {
            return await _context.Abonnementen
                .Include(a => a.Bedrijf)
                .FirstOrDefaultAsync(a => a.AbonnementID == id);
        }

        // Haal alle abonnementen op
        public async Task<List<Abonnement>> GetAlleAbonnementenAsync()
        {
            return await _context.Abonnementen
                .Include(a => a.Bedrijf)
                .ToListAsync();
        }

        // Voeg een nieuw abonnement toe
        public async Task AddAbonnementAsync(Abonnement nieuwAbonnement)
        {
            _context.Abonnementen.Add(nieuwAbonnement);
            await _context.SaveChangesAsync();
        }

        // Werk een abonnement bij
        public async Task UpdateAbonnementAsync(int id, Abonnement bijgewerktAbonnement)
        {
            var bestaandAbonnement = await _context.Abonnementen.FindAsync(id);
            if (bestaandAbonnement == null) throw new Exception("Abonnement niet gevonden.");

            bestaandAbonnement.Type = bijgewerktAbonnement.Type;
            bestaandAbonnement.EindDatum = bijgewerktAbonnement.EindDatum;

            await _context.SaveChangesAsync();
        }

        // Verwijder een abonnement
        public async Task DeleteAbonnementAsync(int id)
        {
            var abonnement = await _context.Abonnementen.FindAsync(id);
            if (abonnement == null) throw new Exception("Abonnement niet gevonden.");

            _context.Abonnementen.Remove(abonnement);
            await _context.SaveChangesAsync();
        }
    }
}
