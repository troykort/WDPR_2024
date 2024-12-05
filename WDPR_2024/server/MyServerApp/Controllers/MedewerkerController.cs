using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WDPR_2024.server.MyServerApp.Data;
using WDPR_2024.server.MyServerApp.Models;
using WDPR_2024.server.MyServerApp.Services;

namespace WDPR_2024.server.MyServerApp.Controllers
{
    [ApiController]
    [Route("api/medewerkers")]
    [Authorize(Roles = "Backoffice")]
    public class MedewerkerController : ControllerBase
    {
        private readonly MedewerkerService _medewerkerService;

        public MedewerkerController(MedewerkerService medewerkerService)
        {
            _medewerkerService = medewerkerService;
        }

        // 1. GET: Haal alle medewerkers op
        [HttpGet]
        public async Task<IActionResult> GetAlleMedewerkers()
        {
            var medewerkers = await _medewerkerService.GetAlleMedewerkersAsync();
            return Ok(medewerkers);
        }

        // 2. POST: Voeg een nieuwe medewerker toe
        [HttpPost]
        public async Task<IActionResult> CreateMedewerker(Klant nieuweMedewerker, [FromQuery] string rol)
        {
            try
            {
                await _medewerkerService.AddMedewerkerAsync(nieuweMedewerker, rol);
                return Ok("Medewerker succesvol toegevoegd.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 3. PUT: Wijzig de rol van een medewerker
        [HttpPut("{id}/rol")]
        public async Task<IActionResult> UpdateMedewerkerRol(int id, [FromQuery] string nieuweRol)
        {
            try
            {
                await _medewerkerService.UpdateMedewerkerRolAsync(id, nieuweRol);
                return Ok($"Rol succesvol bijgewerkt naar {nieuweRol}.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 4. DELETE: Verwijder een medewerker
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedewerker(int id)
        {
            try
            {
                await _medewerkerService.DeleteMedewerkerAsync(id);
                return Ok("Medewerker succesvol verwijderd.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}

}
