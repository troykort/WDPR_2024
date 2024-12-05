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
    [Route("api/verhuur-aanvragen")]
    public class VerhuurAanvraagController : ControllerBase
    {
        private readonly VerhuurAanvraagService _aanvraagService;

        public VerhuurAanvraagController(VerhuurAanvraagService aanvraagService)
        {
            _aanvraagService = aanvraagService;
        }

        // 1. GET: Haal een specifieke aanvraag op
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAanvraag(int id)
        {
            var aanvraag = await _aanvraagService.GetAanvraagByIdAsync(id);
            if (aanvraag == null) return NotFound("Aanvraag niet gevonden.");

            return Ok(aanvraag);
        }

        // 2. GET: Haal alle aanvragen op
        [Authorize(Roles = "Backoffice")]
        [HttpGet]
        public async Task<IActionResult> GetAlleAanvragen()
        {
            var aanvragen = await _aanvraagService.GetAlleAanvragenAsync();
            return Ok(aanvragen);
        }

        // 3. POST: Voeg een nieuwe aanvraag toe
        [HttpPost]
        public async Task<IActionResult> CreateAanvraag(VerhuurAanvraag nieuweAanvraag)
        {
            try
            {
                await _aanvraagService.AddAanvraagAsync(nieuweAanvraag);
                return Ok("Aanvraag succesvol aangemaakt.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 4. PUT: Werk de status van een aanvraag bij
        [Authorize(Roles = "Backoffice")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string nieuweStatus, [FromQuery] string opmerkingen = null)
        {
            try
            {
                await _aanvraagService.UpdateAanvraagStatusAsync(id, nieuweStatus, opmerkingen);
                return Ok($"Status succesvol bijgewerkt naar {nieuweStatus}.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 5. GET: Haal aanvragen op basis van status
        [Authorize(Roles = "Backoffice")]
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetAanvragenByStatus(string status)
        {
            var aanvragen = await _aanvraagService.GetAanvragenByStatusAsync(status);
            if (aanvragen == null || aanvragen.Count == 0) return NotFound($"Geen aanvragen gevonden met status: {status}.");

            return Ok(aanvragen);
        }
    }
}
