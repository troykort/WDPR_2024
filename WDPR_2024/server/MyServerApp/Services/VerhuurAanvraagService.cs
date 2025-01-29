using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WDPR_2024.server.MyServerApp.Data;
using WDPR_2024.server.MyServerApp.DtoModels;
using WDPR_2024.server.MyServerApp.Models;

namespace WDPR_2024.server.MyServerApp.Services
{
    public class VerhuurAanvraagService
    {
        private readonly AppDbContext _context;

        public VerhuurAanvraagService(AppDbContext context)
        {
            _context = context;
        }

        // Haal een specifieke verhuuraanvraag op
        public async Task<VerhuurAanvraag> GetAanvraagByIdAsync(int id)
        {
            return await _context.VerhuurAanvragen
                .Include(a => a.Klant)
                .Include(a => a.Voertuig)
                .FirstOrDefaultAsync(a => a.VerhuurAanvraagID == id);
        }

        // Haal alle aanvragen op
        public async Task<List<VerhuurAanvraag>> GetAlleAanvragenAsync()
        {
            return await _context.VerhuurAanvragen
                .Include(a => a.Klant)
                .Include(a => a.Voertuig)
                .ToListAsync();
        }


        public async Task AddAanvraagAsync(VerhuurAanvraag nieuweAanvraag)
        {
            
            var voertuig = await _context.Voertuigen.FindAsync(nieuweAanvraag.VoertuigID);

            
            if (voertuig == null || voertuig.Status != "Beschikbaar")
                throw new Exception("Het geselecteerde voertuig is niet beschikbaar.");

            
            nieuweAanvraag.Status = "In Behandeling";
            voertuig.Status = "In Behandeling";

            
            _context.VerhuurAanvragen.Add(nieuweAanvraag);

            
            await _context.SaveChangesAsync();
        }


        // Werk een aanvraagstatus bij (Goedgekeurd, Afgewezen, Uitgegeven)
        public async Task UpdateAanvraagStatusAsync(int id, string nieuweStatus, string opmerkingenTekst = null)
        {
          
            var aanvraag = await _context.VerhuurAanvragen
                .Include(a => a.Opmerkingen) 
                .Include(a => a.Voertuig) 
                .FirstOrDefaultAsync(a => a.VerhuurAanvraagID == id);

            if (aanvraag == null) throw new Exception("Aanvraag niet gevonden.");

           
            aanvraag.Status = nieuweStatus;

            
            if (!string.IsNullOrWhiteSpace(opmerkingenTekst))
            {
                var opmerking = new Opmerking
                {
                    VerhuurAanvraagID = id,
                    Tekst = opmerkingenTekst,
                    GebruikerNaam = "Backoffice/Frontoffice gebruiker", 
                    DatumToegevoegd = DateTime.UtcNow
                };
                _context.Opmerkingen.Add(opmerking);
            }

           
            if (nieuweStatus == "Goedgekeurd")
            {
                var voertuig = aanvraag.Voertuig;
                if (voertuig == null) throw new Exception("Voertuig niet gevonden.");
                voertuig.Status = "Verhuurd";
            }

            
            if (nieuweStatus == "Uitgegeven")
            {
                var voertuig = aanvraag.Voertuig;
                if (voertuig == null) throw new Exception("Voertuig niet gevonden.");

                
                voertuig.Status = "Uitgegeven";

                
                aanvraag.Uitgiftedatum = DateTime.Now;
            }

            await _context.SaveChangesAsync(); 
        }


        // Haal aanvragen op basis van status
        public async Task<List<VerhuurAanvraag>> GetAanvragenByStatusAsync(string status)
        {
            return await _context.VerhuurAanvragen
                .Where(a => a.Status == status)
                .Include(a => a.Klant)
                .Include(a => a.Voertuig)
                .ToListAsync();
        }

        // Haal verhuurde voertuigen op met filters
        public async Task<List<VerhuurAanvraag>> GetVerhuurdeVoertuigenAsync(DateTime? startDate, DateTime? endDate, string? voertuigType, string? huurderNaam)
        {
            var query = _context.VerhuurAanvragen
                .Include(v => v.Voertuig)
                .Include(v => v.Klant)
                .Where(v => v.Status == "Verhuurd")
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(v => v.StartDatum >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(v => v.EindDatum <= endDate.Value);
            if (!string.IsNullOrEmpty(voertuigType))
                query = query.Where(v => v.Voertuig.TypeVoertuig == voertuigType);
            if (!string.IsNullOrEmpty(huurderNaam))
                query = query.Where(v => v.Klant.Naam.Contains(huurderNaam));

            return await query.ToListAsync();
        }

        public async Task VoegOpmerkingToeAsync(Opmerking opmerking)
        {
            _context.Opmerkingen.Add(opmerking);
            await _context.SaveChangesAsync();
        }


        // Haal alle beschikbare voertuigen op basis van type en datums
        public async Task<List<Voertuig>> GetBeschikbareVoertuigenAsync(string type, DateTime startDatum, DateTime eindDatum)
        {
            var voertuigen = await _context.Voertuigen
                .Where(v => v.TypeVoertuig == type && v.Status == "Beschikbaar")
                .ToListAsync();

            var verhuurdeVoertuigen = await _context.VerhuurAanvragen
                .Where(a => a.Status == "Verhuurd" &&
                            a.StartDatum < eindDatum && a.EindDatum > startDatum)
                .Select(a => a.VoertuigID)
                .ToListAsync();

            return voertuigen.Where(v => !verhuurdeVoertuigen.Contains(v.VoertuigID)).ToList();
        }

        // Controleer beschikbaarheid van een voertuig
        public async Task<bool> IsVoertuigBeschikbaarAsync(int voertuigId, DateTime startDatum, DateTime eindDatum)
        {
            var overlappendeAanvragen = await _context.VerhuurAanvragen
                .Where(a => a.VoertuigID == voertuigId &&
                            a.Status == "Verhuurd" &&
                            a.StartDatum < eindDatum &&
                            a.EindDatum > startDatum)
                .ToListAsync();

            return !overlappendeAanvragen.Any();
        }

        // Haal details van een voertuig op basis van ID
        public async Task<Voertuig> GetVoertuigDetailsAsync(int voertuigId)
        {
            return await _context.Voertuigen.FirstOrDefaultAsync(v => v.VoertuigID == voertuigId);
        }
         public async Task<List<VerhuurAanvraag>> GetVerhuurGeschiedenisByKlantIdAsync(int klantId)
        {
            var verhuurGeschiedenis = await _context.VerhuurAanvragen
                .Where(a => a.KlantID == klantId && (a.Status == "Goedgekeurd" || a.Status == "Verhuurd"))
                .Include(a => a.Voertuig)
                .Include(a => a.Klant)
                .ToListAsync();

            if (verhuurGeschiedenis == null || verhuurGeschiedenis.Count == 0)
            {
        Console.WriteLine("No rental history found for klantId: {0}", klantId);
            }
            else
            {
        Console.WriteLine("Rental history successfully retrieved for klantId: {0}", klantId);
            }

            return verhuurGeschiedenis;
        }
    }
}