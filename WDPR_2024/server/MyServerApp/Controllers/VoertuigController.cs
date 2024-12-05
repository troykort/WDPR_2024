using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WDPR_2024.server.MyServerApp.Models;
using WDPR_2024.server.MyServerApp.Services;

namespace WDPR_2024.server.MyServerApp.Controllers
{
    [ApiController]
    [Route("api/voertuigen")]
    public class VoertuigController : ControllerBase
    {
        private readonly VoertuigService _voertuigService;

        public VoertuigController(VoertuigService voertuigService)
        {
            _voertuigService = voertuigService;
        }

        // 1. GET: Haal een specifiek voertuig op
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoertuig(int id)
        {
            var voertuig = await _voertuigService.GetVoertuigByIdAsync(id);
            if (voertuig == null) return NotFound("Voertuig niet gevonden.");

            return Ok(voertuig);
        }

        // 2. GET: Haal een lijst van alle voertuigen op
        [Authorize(Roles = "Backoffice")]
        [HttpGet]
        public async Task<IActionResult> GetAlleVoertuigen()
        {
            var voertuigen = await _voertuigService.GetAlleVoertuigenAsync();
            return Ok(voertuigen);
        }

        // 3. POST: Voeg een nieuw voertuig toe
        [HttpPost]
        public async Task<IActionResult> CreateVoertuig(Voertuig nieuwVoertuig)
        {
            try
            {
                await _voertuigService.AddVoertuigAsync(nieuwVoertuig);
                return Ok("Voertuig succesvol aangemaakt.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 4. PUT: Werk een voertuig bij
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoertuig(int id, Voertuig bijgewerktVoertuig)
        {
            try
            {
                await _voertuigService.UpdateVoertuigAsync(id, bijgewerktVoertuig);
                return Ok("Voertuig succesvol bijgewerkt.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 5. DELETE: Verwijder een voertuig
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoertuig(int id)
        {
            try
            {
                await _voertuigService.DeleteVoertuigAsync(id);
                return Ok("Voertuig succesvol verwijderd.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 6. GET: Haal voertuigen op op basis van type (auto, camper, caravan)
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetVoertuigenByType(string type)
        {
            var voertuigen = await _voertuigService.GetVoertuigenByTypeAsync(type);
            if (voertuigen == null || voertuigen.Count == 0) return NotFound("Geen voertuigen gevonden van dit type.");

            return Ok(voertuigen);
        }
    }
}
