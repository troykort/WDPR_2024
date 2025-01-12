using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDPR_2024.server.MyServerApp.Data;
using WDPR_2024.server.MyServerApp.Models;

namespace WDPR_2024.server.MyServerApp.Services
{
    public class VoertuigService
    {
        private readonly AppDbContext _context;

        public VoertuigService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Voertuig>> GetUitgegevenVoertuigenAsync()
        {
            return await _context.VerhuurAanvragen
                .Where(va => va.Status == "Uitgegeven")
                .Include(va => va.Voertuig)
                .Include(va => va.Klant)
                .Select(va => new Voertuig
                {
                    VoertuigID = va.Voertuig.VoertuigID,
                    Merk = va.Voertuig.Merk,
                    Type = va.Voertuig.Type,
                    Kenteken = va.Voertuig.Kenteken,
                    Status = va.Voertuig.Status,
                    HuidigeHuurderID = va.Klant.KlantID,
                    HuidigeHuurderNaam = va.Klant.Naam,
                    HuidigeHuurderEmail = va.Klant.Email,
                    Uitgiftedatum = va.Uitgiftedatum

                })
                .ToListAsync();
        }


        public async Task UpdateVoertuigStatusAsync(int voertuigId, string nieuweStatus)
        {
            var voertuig = await _context.Voertuigen.FindAsync(voertuigId);
            if (voertuig == null) throw new Exception("Voertuig niet gevonden.");

            voertuig.Status = nieuweStatus;
            await _context.SaveChangesAsync();
        }

        // Haal een specifiek voertuig op
        public async Task<Voertuig> GetVoertuigByIdAsync(int id)
        {
            return await _context.Voertuigen
                .FirstOrDefaultAsync(v => v.VoertuigID == id);
        }

        // Haal alle voertuigen op
        public async Task<List<Voertuig>> GetAlleVoertuigenAsync()
        {
            return await _context.Voertuigen.ToListAsync();
        }

        // Voeg een nieuw voertuig toe
        public async Task AddVoertuigAsync(Voertuig nieuwVoertuig)
        {
            _context.Voertuigen.Add(nieuwVoertuig);
            await _context.SaveChangesAsync();
        }

        // Werk een voertuig bij
        public async Task UpdateVoertuigAsync(int id, Voertuig bijgewerktVoertuig)
        {
            var bestaandVoertuig = await _context.Voertuigen.FindAsync(id);
            if (bestaandVoertuig == null) throw new Exception("Voertuig niet gevonden.");

            bestaandVoertuig.Merk = bijgewerktVoertuig.Merk;
            bestaandVoertuig.Type = bijgewerktVoertuig.Type;
            bestaandVoertuig.Kenteken = bijgewerktVoertuig.Kenteken;
            bestaandVoertuig.PrijsPerDag = bijgewerktVoertuig.PrijsPerDag;
            bestaandVoertuig.Status = bijgewerktVoertuig.Status;

            await _context.SaveChangesAsync();
        }

        // Verwijder een voertuig
        public async Task DeleteVoertuigAsync(int id)
        {
            var voertuig = await GetVoertuigByIdAsync(id);
            if (voertuig == null) throw new Exception("Voertuig niet gevonden.");

            _context.Voertuigen.Remove(voertuig);
            await _context.SaveChangesAsync();
        }


        // Voeg filterfunctionaliteit toe
        public async Task<List<Voertuig>> GetVoertuigenByTypeAsync(string TypeVoertuig)
        {
            return await _context.Voertuigen
                .Where(v => v.TypeVoertuig == TypeVoertuig)
                .ToListAsync();
        }


    }
}
