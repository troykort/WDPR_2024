using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WDPR_2024.server.MyServerApp.Services;
using WDPR_2024.server.MyServerApp.Models;


[ApiController]
[Route("api/notificaties")]
public class NotificatieController : ControllerBase
{
    private readonly NotificatieService _notificatieService;
    private readonly EmailService _emailService;

    public NotificatieController(NotificatieService notificatieService, EmailService emailService)
    {
        _notificatieService = notificatieService;
        _emailService = emailService;
    }



    // 2. GET: Haal alle notificaties voor een specifieke klant of medewerker op
    [HttpGet("klant/{Id}")]
    public async Task<IActionResult> GetNotificatiesVoorKlant(int Id)
    { 
        var notificaties = await _notificatieService.GetNotificatiesVoorKlantAsync(Id);
        if (notificaties == null || notificaties.Count == 0) return NotFound("Geen notificaties gevonden voor deze klant.");

        return Ok(notificaties);
    }

    // 3. GET: Haal alle notificaties voor een specifieke medewerker op
    [HttpGet("medewerker/{Id}")]
    public async Task<IActionResult> GetNotificatiesVoorMedewerker(int Id)
    { 
        var notificaties = await _notificatieService.GetNotificatiesVoorMedewerkerAsync(Id);
        if (notificaties == null || notificaties.Count == 0) return NotFound("Geen notificaties gevonden voor deze medewerker.");

        return Ok(notificaties);
    } 
    



    // 4. POST: Voeg een nieuwe notificatie toe
    [HttpPost]
    public async Task<IActionResult> CreateNotificatie(Notificatie nieuweNotificatie)
    {
        try
        {
            await _notificatieService.AddNotificatieAsync(nieuweNotificatie);

            //var klantEmail = nieuweNotificatie.Klant.Email;
            //var subject = nieuweNotificatie.Titel;
            //var body = nieuweNotificatie.Bericht;
            //await _emailService.SendEmailAsync(klantEmail, subject, body);
            return Ok("Notificatie succesvol aangemaakt.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // 5. DELETE: Verwijder een notificatie
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
