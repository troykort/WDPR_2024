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
        public async Task<Schademelding?> GetSchademeldingByIdAsync(int id)
        {
            return await _context.Schademeldingen
                .Include(s => s.Voertuig)
                .Include(s => s.Klant)
                .Include(s => s.VerhuurAanvraag)
                .FirstOrDefaultAsync(s => s.SchademeldingID == id);
        }

        // Haal alle schademeldingen op
        public async Task<List<Schademelding>> GetAllSchademeldingenAsync()
        {
            return await _context.Schademeldingen
                .Include(s => s.Voertuig)
                .Include(s => s.Klant)
                .Include(s => s.VerhuurAanvraag)
                .ToListAsync();
        }

        public async Task<Schademelding> AddSchademeldingAsync(Schademelding schademelding)
        {
            schademelding.Melddatum = DateTime.UtcNow;
            schademelding.Status = "In Behandeling";
            _context.Schademeldingen.Add(schademelding);
            await _context.SaveChangesAsync();
            return schademelding;
        }

        public async Task<string> UploadFotoAsync(IFormFile foto)
        {
            if (foto == null) throw new Exception("Geen foto aangeleverd.");
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "schadefotos");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(foto.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await foto.CopyToAsync(fileStream);
            }

            return $"/schadefotos/{uniqueFileName}";
        }

        // Werk de status van een schademelding bij
        public async Task UpdateSchademeldingStatusAsync(int id, string nieuweStatus, string? opmerkingen)
        {
            var schademelding = await _context.Schademeldingen.FindAsync(id);

            if (schademelding == null)
            {
                throw new Exception("Schademelding niet gevonden.");
            }

            // Werk de status en opmerkingen bij
            schademelding.Status = nieuweStatus;
            if (!string.IsNullOrWhiteSpace(opmerkingen))
            {
                schademelding.Opmerkingen = opmerkingen;
            }

            // Sla de wijzigingen op in de database
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
