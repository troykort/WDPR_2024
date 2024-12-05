using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WDPR_2024.server.MyServerApp.Services;
using WDPR_2024.server.MyServerApp.Models;


[ApiController]
[Route("api/notificaties")]
public class NotificatieController : ControllerBase
{
    private readonly NotificatieService _notificatieService;

    public NotificatieController(NotificatieService notificatieService)
    {
        _notificatieService = notificatieService;
    }

    // 1. GET: Haal een specifieke notificatie op
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNotificatie(int id)
    {
        var notificatie = await _notificatieService.GetNotificatieByIdAsync(id);
        if (notificatie == null) return NotFound("Notificatie niet gevonden.");

        return Ok(notificatie);
    }

    // 2. GET: Haal alle notificaties voor een specifieke klant op
    [HttpGet("klant/{klantId}")]
    public async Task<IActionResult> GetNotificatiesVoorKlant(int klantId)
    {
        var notificaties = await _notificatieService.GetNotificatiesVoorKlantAsync(klantId);
        if (notificaties == null || notificaties.Count == 0) return NotFound("Geen notificaties gevonden voor deze klant.");

        return Ok(notificaties);
    }

    // 3. POST: Voeg een nieuwe notificatie toe
    [HttpPost]
    public async Task<IActionResult> CreateNotificatie(Notificatie nieuweNotificatie)
    {
        try
        {
            await _notificatieService.AddNotificatieAsync(nieuweNotificatie);
            return Ok("Notificatie succesvol aangemaakt.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // 4. DELETE: Verwijder een notificatie
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotificatie(int id)
    {
        try
        {
            await _notificatieService.DeleteNotificatieAsync(id);
            return Ok("Notificatie succesvol verwijderd.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
