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
        private readonly NotificatieService _notificatieService;
        private readonly KlantService _klantService;

        public VerhuurAanvraagController(VerhuurAanvraagService aanvraagService, EmailService emailService, NotificatieService notificatieService)
        {
            _aanvraagService = aanvraagService;
            _emailService = emailService;
            _notificatieService = notificatieService;
        }

        [Authorize(Roles = "Backoffice, Frontoffice")]
        [HttpPost("{id}/opmerkingen")]
        public async Task<IActionResult> VoegOpmerkingToe(int id, [FromBody] string tekst)
        {
            try
            {
                var aanvraag = await _aanvraagService.GetAanvraagByIdAsync(id);
                if (aanvraag == null)
                {
                    return NotFound("Verhuur aanvraag niet gevonden.");
                }

                var opmerking = new Opmerking
                {
                    Tekst = tekst,
                    VerhuurAanvraagID = id,
                    GebruikerNaam = User.Identity.Name,
                    DatumToegevoegd = DateTime.UtcNow
                };

                await _aanvraagService.VoegOpmerkingToeAsync(opmerking);

                return Ok(opmerking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
        [Authorize(Roles = "Backoffice, Frontoffice")]
        [HttpPut("{id}/{status}")]
        public async Task<IActionResult> UpdateStatus(int id, string status, [FromBody] UpdateStatusRequest request)
        {
        
            
            try
            {
                
                var aanvraag = await _aanvraagService.GetAanvraagByIdAsync(id);
                if (aanvraag == null)
                {
                    return NotFound("Aanvraag niet gevonden.");
                }

              
                await _aanvraagService.UpdateAanvraagStatusAsync(id, status, request.Opmerkingen);
                
                _notificatieService.AddNotificatieAsync(new Notificatie
                {
                    KlantID = request.KlantID,
                    Titel = "verhuuraanvraag update",
                    Bericht = $"De status van aanvraag {id} van {aanvraag.Klant.Naam} is bijgewerkt naar {status}.",
                    VerzondenOp = DateTime.UtcNow,
                    Gelezen = false
                });

                if (status == "Goedgekeurd")
                {
                        aanvraag = await _aanvraagService.GetAanvraagByIdAsync(id);
                        var klant = aanvraag.Klant;
                        var voertuig = aanvraag.Voertuig;
                        var startDatum = aanvraag.StartDatum;
                        var eindDatum = aanvraag.EindDatum;
                        var notificatie = new Notificatie
                        {
                           
                            MedewerkerID = request.MedewerkerID,
                            Titel = $"Verhuuraanvraag {voertuig.Merk} {voertuig.Type}",
                            Bericht = $"Uw verhuuraanvraag voor {voertuig.Merk} {voertuig.Type} is goedgekeurd. U kunt het voertuig ophalen op {startDatum}.",
                            VerzondenOp = DateTime.UtcNow,
                            Gelezen = false
                        };
                    await _notificatieService.AddNotificatieAsync(notificatie);


                } else if (status == "Afgekeurd")
                {
                    aanvraag = await _aanvraagService.GetAanvraagByIdAsync(id);
                    var klant = aanvraag.Klant;
                    var voertuig = aanvraag.Voertuig;
                    var startDatum = aanvraag.StartDatum;
                    var eindDatum = aanvraag.EindDatum;
                    var notificatie = new Notificatie
                    {
                        
                        MedewerkerID = request.MedewerkerID,
                        Titel = $"Verhuuraanvraag {voertuig.Merk} {voertuig.Type}",
                        Bericht = $"Uw verhuuraanvraag voor {voertuig.Merk} {voertuig.Type} is afgekeurd, reden:{request.Opmerkingen}",
                        VerzondenOp = DateTime.UtcNow,
                        Gelezen = false
                    };
                    await _notificatieService.AddNotificatieAsync(notificatie);
                }


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