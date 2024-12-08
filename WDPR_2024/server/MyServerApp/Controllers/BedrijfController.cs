using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WDPR_2024.server.MyServerApp.Models;
using WDPR_2024.server.MyServerApp.Services;

namespace WDPR_2024.server.MyServerApp.Controllers
{
    [ApiController]
    [Route("api/bedrijven")]
    public class BedrijfController : ControllerBase
    {
        private readonly BedrijfService _bedrijfService;

        public BedrijfController(BedrijfService bedrijfService)
        {
            _bedrijfService = bedrijfService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBedrijf(int id)
        {
            var bedrijf = await _bedrijfService.GetBedrijfByIdAsync(id);
            if (bedrijf == null) return NotFound("Bedrijf niet gevonden.");

            return Ok(bedrijf);
        }

        [Authorize(Roles = "Backoffice")]
        [HttpGet]
        public async Task<IActionResult> GetAlleBedrijven()
        {
            var bedrijven = await _bedrijfService.GetAlleBedrijvenAsync();
            return Ok(bedrijven);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBedrijf(Bedrijf nieuwBedrijf)
        {
            try
            {
                await _bedrijfService.AddBedrijfAsync(nieuwBedrijf);
                return Ok("Bedrijf succesvol aangemaakt.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBedrijf(int id, Bedrijf bijgewerktBedrijf)
        {
            try
            {
                await _bedrijfService.UpdateBedrijfAsync(id, bijgewerktBedrijf);
                return Ok("Bedrijfsgegevens succesvol bijgewerkt.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBedrijf(int id)
        {
            try
            {
                await _bedrijfService.DeleteBedrijfAsync(id);
                return Ok("Bedrijf succesvol verwijderd.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/medewerkers")]
        public async Task<IActionResult> GetMedewerkersVanBedrijf(int id)
        {
            var medewerkers = await _bedrijfService.GetMedewerkersVanBedrijfAsync(id);
            if (medewerkers == null || medewerkers.Count == 0) return NotFound("Geen medewerkers gevonden voor dit bedrijf.");

            return Ok(medewerkers);
        }
    }
}
