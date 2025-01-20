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

      

        // Haal alle notificaties voor iemand
        public async Task<List<Notificatie>> GetUserNotificatiesAsync(string userId)
        {
            return await _context.Notificaties
                .Where(n => n.userID == userId)
                .OrderByDescending(n => n.VerzondenOp)
                .ToListAsync();
        }

        // Voeg een nieuwe notificatie toe
        public async Task AddNotificatieAsync(Notificatie notificatie)
        {
            await _context.Notificaties.AddAsync(notificatie);
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
