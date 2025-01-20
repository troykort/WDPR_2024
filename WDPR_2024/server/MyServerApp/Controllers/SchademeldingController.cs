using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WDPR_2024.server.MyServerApp.Services;
using WDPR_2024.server.MyServerApp.Models;
using Microsoft.EntityFrameworkCore;
using WDPR_2024.server.MyServerApp.DtoModels;
using System.Text.Json;

[ApiController]
[Route("api/schademeldingen")]
public class SchademeldingController : ControllerBase
{
    private readonly SchademeldingService _schademeldingService;

    public SchademeldingController(SchademeldingService schademeldingService)
    {
        _schademeldingService = schademeldingService;
    }

    // 1. GET: Haal een specifieke schademelding op
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSchademeldingById(int id)
    {
        var melding = await _schademeldingService.GetSchademeldingByIdAsync(id);
        if (melding == null)
            return NotFound("Schademelding niet gevonden.");
        return Ok(melding);
    }

    // 2. GET: Haal alle schademeldingen op
    // 2. GET: Haal alle schademeldingen op
    [Authorize(Roles = "Backoffice")]
    [HttpGet]
    public async Task<IActionResult> GetSchademeldingen()
    {
        var schademeldingen = await _schademeldingService.GetAllSchademeldingenAsync();
        var schademeldingendto = schademeldingen.Select(s => new SchademeldingDto
        {
            SchademeldingID = s.SchademeldingID,
            VoertuigID = s.VoertuigID,
            KlantID = s.KlantID,
            KlantNaam = s.Klant.Naam,
            VoertuigMerk = s.Voertuig.Merk,
            VoertuigType = s.Voertuig.Type,
            Beschrijving = s.Beschrijving,
            Opmerkingen = s.Opmerkingen,
            FotoPath = s.FotoPath,
            Melddatum = s.Melddatum,
            Status = s.Status
        })
        .ToList();

     

        return Ok(schademeldingendto);
    }






    // 3. POST: Voeg een nieuwe schademelding toe
    [HttpPost]
    public async Task<IActionResult> AddSchademelding([FromBody] Schademelding schademelding)
    {
        try
        {
            var newMelding = await _schademeldingService.AddSchademeldingAsync(schademelding);
            return Ok(newMelding);
        }
        catch (Exception ex)
        {
            return BadRequest($"Fout bij toevoegen schademelding: {ex.Message}");
        }
    }

    // 4. PUT: Werk de status van een schademelding bij
    [Authorize(Roles = "Backoffice")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateSchademeldingStatus(int id, [FromQuery] string nieuweStatus, [FromQuery] string opmerkingen = null)
    {
        try
        {
            // Roep de service aan om de schademeldingstatus bij te werken
            await _schademeldingService.UpdateSchademeldingStatusAsync(id, nieuweStatus, opmerkingen);

            // Retourneer een succesbericht
            return Ok($"Status succesvol bijgewerkt naar {nieuweStatus}.");
        }
        catch (Exception ex)
        {
            // Retourneer een foutmelding bij een mislukte operatie
            return BadRequest($"Fout bij het bijwerken van de status: {ex.Message}");
        }
    }


    // 5. DELETE: Verwijder een schademelding
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchademelding(int id)
    {
        try
        {
            await _schademeldingService.DeleteSchademeldingAsync(id);
            return Ok("Schademelding succesvol verwijderd.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
