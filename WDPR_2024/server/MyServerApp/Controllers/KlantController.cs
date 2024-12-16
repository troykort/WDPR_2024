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


[HttpPut("{id}")]
public async Task<IActionResult> UpdateKlant(int id, [FromBody] Klant gewijzigdeKlant)
{
    try
    {
        var existingKlant = await _klantService.GetKlantByIdAsync(id);
        if (existingKlant == null)
        {
            return NotFound("Klant niet gevonden.");
        }

        // Dynamically update all fields
        if (!string.IsNullOrEmpty(gewijzigdeKlant.Naam))
            existingKlant.Naam = gewijzigdeKlant.Naam;

        if (!string.IsNullOrEmpty(gewijzigdeKlant.Adres))
            existingKlant.Adres = gewijzigdeKlant.Adres;

        if (!string.IsNullOrEmpty(gewijzigdeKlant.Telefoonnummer))
            existingKlant.Telefoonnummer = gewijzigdeKlant.Telefoonnummer;

        if (!string.IsNullOrEmpty(gewijzigdeKlant.Email))
            existingKlant.Email = gewijzigdeKlant.Email;

        if (!string.IsNullOrEmpty(gewijzigdeKlant.Wachtwoord))
            existingKlant.Wachtwoord = _klantService.HashPassword(gewijzigdeKlant.Wachtwoord);

        await _klantService.UpdateKlantAsync(id, existingKlant);
        return Ok("Klantgegevens succesvol bijgewerkt.");
    }
    catch (Exception ex)
    {
        return BadRequest($"Fout bij bijwerken van klant: {ex.Message}");
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
      [HttpGet("details/{klantId}")]
public async Task<IActionResult> GetDetails(int klantId)
{
    try
    {
        Console.WriteLine($"Received KlantID: {klantId}"); // Debugging log

        // Fetch the Klant details using KlantID
        var klant = await _klantService.GetKlantByIdAsync(klantId);
        if (klant == null)
            return NotFound("Klant niet gevonden.");

        // Return the Klant details as a DTO
        var klantDto = new KlantDto
        {
            KlantID = klant.KlantID,
            Naam = klant.Naam,
            Adres = klant.Adres,
            Telefoonnummer = klant.Telefoonnummer,
            Email = klant.Email
        };

        return Ok(klantDto);
    }
    catch (Exception ex)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, $"Er is een fout opgetreden: {ex.Message}");
    }
}




        
    }
}
