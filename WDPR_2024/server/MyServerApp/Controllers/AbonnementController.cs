using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WDPR_2024.server.MyServerApp.Models;
using WDPR_2024.server.MyServerApp.Services;

namespace WDPR_2024.server.MyServerApp.Controllers
{
    [ApiController]
    [Route("api/abonnementen")]
    public class AbonnementController : ControllerBase
    {
        private readonly AbonnementService _abonnementService;

        public AbonnementController(AbonnementService abonnementService)
        {
            _abonnementService = abonnementService;
        }

        // 1. GET: Haal een specifiek abonnement op
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAbonnement(int id)
        {
            var abonnement = await _abonnementService.GetAbonnementByIdAsync(id);
            if (abonnement == null) return NotFound("Abonnement niet gevonden.");

            return Ok(abonnement);
        }

        // 2. GET: Haal alle abonnementen op
        [Authorize(Roles = "Backoffice")]
        [HttpGet]
        public async Task<IActionResult> GetAlleAbonnementen()
        {
            var abonnementen = await _abonnementService.GetAlleAbonnementenAsync();
            return Ok(abonnementen);
        }

        // 3. POST: Voeg een nieuw abonnement toe
        [HttpPost]
        public async Task<IActionResult> CreateAbonnement(Abonnement nieuwAbonnement)
        {
            try
            {
                await _abonnementService.AddAbonnementAsync(nieuwAbonnement);
                return Ok("Abonnement succesvol aangemaakt.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 4. PUT: Werk een abonnement bij
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAbonnement(int id, Abonnement bijgewerktAbonnement)
        {
            try
            {
                await _abonnementService.UpdateAbonnementAsync(id, bijgewerktAbonnement);
                return Ok("Abonnement succesvol bijgewerkt.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 5. DELETE: Verwijder een abonnement
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbonnement(int id)
        {
            try
            {
                await _abonnementService.DeleteAbonnementAsync(id);
                return Ok("Abonnement succesvol verwijderd.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
