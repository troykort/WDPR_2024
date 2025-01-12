using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WDPR_2024.server.MyServerApp.Models;
using WDPR_2024.server.MyServerApp.Services;
using System.Threading.Tasks;
using System;
using WDPR_2024.server.MyServerApp.DtoModels;

namespace WDPR_2024.server.MyServerApp.Controllers
{
    [ApiController]
    [Route("api/verhuur-aanvragen")]
    public class VerhuurAanvraagController : ControllerBase
    {
        private readonly VerhuurAanvraagService _aanvraagService;
        private readonly EmailService _emailService;

        public VerhuurAanvraagController(VerhuurAanvraagService aanvraagService, EmailService emailService)
        {
            _aanvraagService = aanvraagService;
            _emailService = emailService;
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
        [Authorize(Roles = "Backoffice, Frontoffice")]
        [HttpGet]
        public async Task<ActionResult<List<verhuurAanvraagDto>>> GetAlleAanvragen()
        {
            var aanvragen = await _aanvraagService.GetAlleAanvragenAsync();

            // Map entities to DTOs
            var aanvragenDto = aanvragen.Select(a => new verhuurAanvraagDto
            {
                VerhuurAanvraagID = a.VerhuurAanvraagID,
                KlantNaam = a.Klant?.Naam ?? "Onbekend",
                VoertuigInfo = a.Voertuig != null ? $"{a.Voertuig.Merk} {a.Voertuig.Type}" : "Onbekend voertuig",
                StartDatum = a.StartDatum,
                EindDatum = a.EindDatum,
                Status = a.Status
            }).ToList();

            return Ok(aanvragenDto);
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
        [HttpPut("{id}/{status}")]
        public async Task<IActionResult> UpdateStatus(int id, string status, string opmerkingen = null)
        {
            try
            {
                var aanvraag = await _aanvraagService.GetAanvraagByIdAsync(id);

                // Verwerk statusupdate
                await _aanvraagService.UpdateAanvraagStatusAsync(id, status, opmerkingen);


                //// Verstuur een e-mail naar de klant als de aanvraag goedgekeurd is
                //if (status == "Goedgekeurd")
                //{
                //    var klantEmail = aanvraag.Klant.Email;
                //    var klantSubject = "Je verhuuraanvraag is goedgekeurd!";
                //    var klantBody = $"Je verhuuraanvraag is goedgekeurd! Je kunt het voertuig ophalen op de afgesproken datum. Bedankt voor je reservering!";
                //    await _emailService.SendEmailAsync(klantEmail, klantSubject, klantBody);

                //    // Verstuur een e-mail naar de backoffice om hen op de hoogte te stellen van de goedkeuring
                //    var backofficeEmail = "dlamericaan@gmail.com"; // Vervang dit door het e-mailadres van de backoffice
                //    var backofficeSubject = "Nieuwe goedgekeurde verhuuraanvraag";
                //    var backofficeBody = $"Er is een verhuuraanvraag goedgekeurd voor {aanvraag.Klant.Naam}. De aanvraag betreft het voertuig {aanvraag.Voertuig.Type}.";
                //    await _emailService.SendEmailAsync(backofficeEmail, backofficeSubject, backofficeBody);
                //}
                //else if (status == "Afgewezen")
                //{
                //    // Verstuur een e-mail naar de klant als de aanvraag afgewezen is
                //    var klantEmail = aanvraag.Klant.Email;
                //    var klantSubject = "Je verhuuraanvraag is afgewezen";
                //    var klantBody = $"Je verhuuraanvraag is afgewezen. Neem contact op met de backoffice voor meer informatie.";
                //    await _emailService.SendEmailAsync(klantEmail, klantSubject, klantBody);
                //}

                return Ok($"Status succesvol bijgewerkt naar {status}.");
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

        // 6. GET: Haal verhuurde voertuigen op
        [Authorize(Roles = "Wagenparkbeheerder")]
        [HttpGet("verhuurd")]
        public async Task<IActionResult> GetVerhuurdeVoertuigen([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string? voertuigType, [FromQuery] string? huurderNaam)
        {
            var verhuurAanvragen = await _aanvraagService.GetVerhuurdeVoertuigenAsync(startDate, endDate, voertuigType, huurderNaam);
            return Ok(verhuurAanvragen);
        }

      

        // 8. GET: Haal details van een voertuig op
        [HttpGet("voertuig/{voertuigID}")]
        public async Task<IActionResult> GetVoertuigDetails(int voertuigID)
        {
            var voertuig = await _aanvraagService.GetVoertuigDetailsAsync(voertuigID);
            if (voertuig == null) return NotFound("Voertuig niet gevonden.");
            return Ok(voertuig);
        }

        // 9. GET: Controleer beschikbaarheid van een voertuig
        [HttpGet("beschikbaarheid/{voertuigID}")]
        public async Task<IActionResult> CheckBeschikbaarheid(int voertuigID, [FromQuery] DateTime startDatum, [FromQuery] DateTime eindDatum)
        {
            var beschikbaar = await _aanvraagService.IsVoertuigBeschikbaarAsync(voertuigID, startDatum, eindDatum);
            return Ok(new { Beschikbaar = beschikbaar });
        }
    }
}