using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WDPR_2024.server.MyServerApp.Data;
using WDPR_2024.server.MyServerApp.Models;

namespace WDPR_2024.server.MyServerApp.Services
{
    public class BedrijfService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BedrijfService(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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

      
        public async Task AddMedewerkerToBedrijfAsync(int bedrijfId, Klant nieuweMedewerker)
        {
            var bedrijf = await GetBedrijfByIdAsync(bedrijfId);
            if (bedrijf == null) throw new Exception("Bedrijf niet gevonden.");

            
            if (!nieuweMedewerker.Email.EndsWith($"@{bedrijf.EmailDomein}"))
            {
                throw new Exception("Het e-mailadres komt niet overeen met het e-maildomein van het bedrijf.");
            }

            
            nieuweMedewerker.BedrijfID = bedrijfId;
            _context.Klanten.Add(nieuweMedewerker);

            
            var user = new ApplicationUser
            {
                UserName = nieuweMedewerker.Email,
                Email = nieuweMedewerker.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, nieuweMedewerker.Wachtwoord);
            if (!result.Succeeded)
            {
                throw new Exception("Fout bij het aanmaken van de medewerker.");
            }

            await _userManager.AddToRoleAsync(user, "Zakelijk");
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

        // Verwijder een bedrijf en bijbehorende medewerkers
        public async Task DeleteBedrijfAsync(int id)
        {
            var bedrijf = await GetBedrijfByIdAsync(id);
            if (bedrijf == null) throw new Exception("Bedrijf niet gevonden.");

           
            foreach (var medewerker in bedrijf.Medewerkers)
            {
                var user = await _userManager.FindByEmailAsync(medewerker.Email);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
            }

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
