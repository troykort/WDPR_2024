using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WDPR_2024.server.MyServerApp.Models;
using WDPR_2024.server.MyServerApp.Data;
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

        [Authorize(Roles = "Wagenparkbeheerder")]
        [HttpGet("{emailDomain}")]
        public async Task<IActionResult> GetAlleKlanten([FromQuery] string emailDomain)
        {
            var klanten = await _klantService.GetKlantenByEmailDomainAsync(emailDomain);
            return Ok(klanten);
        }

        // 3. POST: Voeg een nieuwe klant toe
        [HttpPost]
        public async Task<IActionResult> CreateKlant(Klant nieuweKlant)
        {
            try
            {
                await _klantService.AddKlantAsync(nieuweKlant);
                return Ok("Klant succesvol geregistreerd.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

        // 6. POST: Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] KlantDto loginData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Authenticate the user using KlantDto
                var authenticatedKlant = await _klantService.AuthenticateKlantAsync(loginData);
                return Ok(authenticatedKlant); // Return safe DTO data
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        // 7. POST: Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] KlantDto registerData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
             
                if (await _klantService.IsEmailGeregistreerdAsync(registerData.Email))
                {
                    return BadRequest("E-mailadres is al geregistreerd.");
                }

                
                var nieuweKlant = new Klant
                {
                    Naam = registerData.Naam,
                    Adres = registerData.Adres,
                    Telefoonnummer = registerData.Telefoonnummer,
                    Email = registerData.Email,
                    Wachtwoord = registerData.Password
                };

                
                await _klantService.AddKlantAsync(nieuweKlant);
                return Ok("Registratie succesvol.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Er is een fout opgetreden tijdens de registratie: {ex.Message}");
            }
        }

        [Authorize(Roles = "Wagenparkbeheerder")]
        [HttpGet("me")]
        public async Task<IActionResult> GetWagenparkbeheerderDetails()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (userIdClaim == null) return Unauthorized("Gebruikers-ID ontbreekt in de tokenclaims.");

            if (!int.TryParse(userIdClaim.Value, out int userId)) return BadRequest("Ongeldig gebruikers-ID.");

            var beheerder = await _klantService.GetKlantByIdAsync(userId);
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
