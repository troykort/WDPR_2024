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



    

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddNotificatie([FromBody] Notificatie notificatie)
    {
        try
        {
            await _notificatieService.AddNotificatieAsync(notificatie);
            return Ok("Notificatie succesvol toegevoegd.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Fout bij het toevoegen van notificatie: {ex.Message}");
        }
    }

    [Authorize]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserNotificaties(string userId)
    {
        try
        {
            var notificaties = await _notificatieService.GetUserNotificatiesAsync(userId);
            return Ok(notificaties);
        }
        catch (Exception ex)
        {
            return BadRequest($"Fout bij het ophalen van notificaties: {ex.Message}");
        }
    }
    [HttpPut("{id}/gelezen")]
    public async Task<IActionResult> UpdateGelezenStatus(int id)
    {
        try
        {
            var gelezen = true;
            await _notificatieService.UpdateNotificatieGelezenStatusAsync(id, gelezen);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



}
