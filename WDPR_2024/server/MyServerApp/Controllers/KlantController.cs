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

[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

   try
    {
        var klant = await _klantService.AuthenticateKlantAsync(loginRequest.Email, loginRequest.Password);
        return Ok(new { klant.KlantID, klant.Naam, klant.Email });
    }
    catch (UnauthorizedAccessException ex)
    {
        return Unauthorized(ex.Message);
    }

        


    }
}
