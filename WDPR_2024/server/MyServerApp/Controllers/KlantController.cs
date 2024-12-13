using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WDPR_2024.server.MyServerApp.Models;
using WDPR_2024.server.MyServerApp.Services;
using WDPR_2024.server.MyServerApp.DtoModels;

namespace WDPR_2024.server.MyServerApp.Controllers
{
    [ApiController]
    [Route("api/klanten")]
    public class KlantController : ControllerBase
    {
        private readonly KlantService _klantService;

        public KlantController(KlantService klantService)
        {
            _klantService = klantService;
        }

        // 1. GET: Haal een specifieke klant op
        [HttpGet("{id}")]
        public async Task<IActionResult> GetKlant(int id)
        {
            var klant = await _klantService.GetKlantByIdAsync(id);
            if (klant == null) return NotFound("Klant niet gevonden.");

            return Ok(klant);
        }

        // 2. GET: Haal alle klanten op
        [HttpGet]
        public async Task<IActionResult> GetAlleKlanten()
        {
            var klanten = await _klantService.GetAlleKlantenAsync();
            return Ok(klanten);
        }


        // 4. PUT: Werk een klant bij
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKlant(int id, Klant gewijzigdeKlant)
        {
            try
            {
                await _klantService.UpdateKlantAsync(id, gewijzigdeKlant);
                return Ok("Klantgegevens succesvol bijgewerkt.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 5. DELETE: Verwijder een klant
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKlant(int id)
        {
            try
            {
                await _klantService.DeleteKlantAsync(id);
                return Ok("Klant succesvol verwijderd.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 8. GET: Wagenparkbeheerder details ophalen
        [Authorize(Roles = "Wagenparkbeheerder")]
        [HttpGet("me")]
        public async Task<IActionResult> GetWagenparkbeheerderDetails()
        {
            var userEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail)) return Unauthorized("Gebruikersinformatie ontbreekt.");

            var beheerder = await _klantService.GetKlantByEmailAsync(userEmail);
            if (beheerder == null) return NotFound("Gebruiker niet gevonden.");

            return Ok(new
            {
                beheerder.KlantID,
                beheerder.Naam,
                beheerder.Email
            });
        }
    }
}
