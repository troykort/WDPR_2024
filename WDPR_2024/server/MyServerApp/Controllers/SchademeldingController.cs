using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WDPR_2024.server.MyServerApp.Services;
using WDPR_2024.server.MyServerApp.Models;

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
    public async Task<IActionResult> GetSchademelding(int id)
    {
        var schademelding = await _schademeldingService.GetSchademeldingByIdAsync(id);
        if (schademelding == null) return NotFound("Schademelding niet gevonden.");

        return Ok(schademelding);
    }

    // 2. GET: Haal alle schademeldingen op
    [Authorize(Roles = "Backoffice")]
    [HttpGet]
    public async Task<IActionResult> GetAlleSchademeldingen()
    {
        var schademeldingen = await _schademeldingService.GetAlleSchademeldingenAsync();
        return Ok(schademeldingen);
    }

    // 3. POST: Voeg een nieuwe schademelding toe
    [HttpPost]
    public async Task<IActionResult> CreateSchademelding(Schademelding nieuweSchademelding)
    {
        try
        {
            await _schademeldingService.AddSchademeldingAsync(nieuweSchademelding);
            return Ok("Schademelding succesvol toegevoegd.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // 4. PUT: Werk de status van een schademelding bij
    [Authorize(Roles = "Backoffice")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateSchademeldingStatus(int id, [FromQuery] string nieuweStatus, [FromQuery] string opmerkingen = null)
    {
        try
        {
            await _schademeldingService.UpdateSchademeldingStatusAsync(id, nieuweStatus, opmerkingen);
            return Ok($"Status succesvol bijgewerkt naar {nieuweStatus}.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
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
