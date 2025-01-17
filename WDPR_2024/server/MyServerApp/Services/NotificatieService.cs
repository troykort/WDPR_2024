using WDPR_2024.server.MyServerApp.Data;
using WDPR_2024.server.MyServerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace WDPR_2024.server.MyServerApp.Services
{
    public class NotificatieService
    {
        private readonly AppDbContext _context;

        public NotificatieService(AppDbContext context)
        {
            _context = context;
        }

        // Haal een specifieke notificatie op
        public async Task<Notificatie> GetNotificatieByIdAsync(int id)
        {
            return await _context.Notificaties
                .FirstOrDefaultAsync(n => n.NotificatieID == id);
        }

        // Haal alle notificaties op voor een specifieke klant
        public async Task<List<Notificatie>> GetNotificatiesVoorKlantAsync(int KlantID)
        {
            return await _context.Notificaties
                .Where(n => n.KlantID == KlantID)
                .ToListAsync();
        }
        // Haal alle notificaties op voor een specifieke medewerker
        public async Task<List<Notificatie>> GetNotificatiesVoorMedewerkerAsync(int MedewerkerID)
        {
            return await _context.Notificaties
                .Where(n => n.MedewerkerID == MedewerkerID)
                .ToListAsync();
        }

        // Voeg een nieuwe notificatie toe
        public async Task AddNotificatieAsync(Notificatie nieuweNotificatie)
        {
            _context.Notificaties.Add(nieuweNotificatie);
            await _context.SaveChangesAsync();
        }

        // Verwijder een notificatie
        public async Task DeleteNotificatieAsync(int id)
        {
            var notificatie = await _context.Notificaties.FindAsync(id);
            if (notificatie == null) throw new Exception("Notificatie niet gevonden.");

            _context.Notificaties.Remove(notificatie);
            await _context.SaveChangesAsync();
        }
    }
}
