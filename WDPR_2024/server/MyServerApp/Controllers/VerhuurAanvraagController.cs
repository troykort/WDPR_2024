using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WDPR_2024.server.MyServerApp.Models;
using WDPR_2024.server.MyServerApp.Services;
using System.Threading.Tasks;
using System;

namespace WDPR_2024.server.MyServerApp.Controllers
{
    [ApiController]
    [Route("api/verhuur-aanvragen")]
    public class VerhuurAanvraagController : ControllerBase
    {
        private readonly VerhuurAanvraagService _aanvraagService;
        private readonly EmailService _emailService; 
        private readonly NotificatieService _notificatieService;
        public VerhuurAanvraagController(VerhuurAanvraagService aanvraagService, EmailService emailService, NotificatieService notificatieService)
        {
            _aanvraagService = aanvraagService;
            _emailService = emailService;
            _notificatieService = notificatieService;
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
                // Haal de aanvraag op
                var aanvraag = await _aanvraagService.GetAanvraagByIdAsync(id);
                if (aanvraag == null)
                {
                    return NotFound("Aanvraag niet gevonden.");
                }

                // Update de status van de aanvraag
                await _aanvraagService.UpdateAanvraagStatusAsync(id, nieuweStatus, opmerkingen);

                // Maak een notificatie voor de klant
                var notificatieTitel = nieuweStatus == "Goedgekeurd"
                    ? "Uw verhuuraanvraag is goedgekeurd!"
                    : "Uw verhuuraanvraag is afgewezen.";

                var notificatieBericht = nieuweStatus == "Goedgekeurd"
                    ? "Je aanvraag is goedgekeurd. Je kunt het voertuig ophalen op de afgesproken datum."
                    : $"Je aanvraag is afgewezen. Opmerkingen: {opmerkingen}";

                var notificatie = new Notificatie
                {
                    Titel = notificatieTitel,
                    Bericht = notificatieBericht,
                    KlantID = aanvraag.KlantID,
                    VerzondenOp = DateTime.UtcNow
                };

                // Voeg de notificatie toe via de NotificatieService
                await _notificatieService.AddNotificatieAsync(notificatie);

                // Als extra: notificatie voor Backoffice (indien nodig)
                if (nieuweStatus == "Goedgekeurd")
                {
                    var backofficeNotificatie = new Notificatie
                    {
                        Titel = "Nieuwe goedgekeurde verhuuraanvraag",
                        Bericht = $"Aanvraag voor voertuig {aanvraag.Voertuig.Type} door {aanvraag.Klant.Naam} is goedgekeurd.",
                        VerzondenOp = DateTime.UtcNow
                    };
                    await _notificatieService.AddNotificatieAsync(backofficeNotificatie);
                }

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

        [Authorize(Roles = "Wagenparkbeheerder")]
        [HttpGet("verhuurd")]
        public async Task<IActionResult> GetVerhuurdeVoertuigen([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string? voertuigType, [FromQuery] string? huurderNaam)
        {
            var verhuurAanvragen = await _aanvraagService.GetVerhuurdeVoertuigenAsync(startDate, endDate, voertuigType, huurderNaam);
            return Ok(verhuurAanvragen);
        }

    }
}
