using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WDPR_2024.server.MyServerApp.Models;
using WDPR_2024.server.MyServerApp.Services;
using WDPR_2024.server.MyServerApp.DtoModels;

namespace WDPR_2024.server.MyServerApp.Controllers
{
    [ApiController]
    [Route("api/voertuiginname")]
    public class VoertuiginnameController : ControllerBase
    {
        private readonly VoertuigService _voertuigService;
        private readonly SchademeldingService _schademeldingService;

        public VoertuiginnameController(VoertuigService voertuigService, SchademeldingService schademeldingService)
        {
            _voertuigService = voertuigService;
            _schademeldingService = schademeldingService;
        }

        [HttpGet("uitgegeven")]
        [Authorize(Roles = "Frontoffice,Backoffice")]
        public async Task<IActionResult> GetUitgegevenVoertuigen()
        {
            try
            {
                var voertuigen = await _voertuigService.GetUitgegevenVoertuigenAsync();
                return Ok(voertuigen);
            }
            catch (Exception ex)
            {
                return BadRequest($"Fout bij het ophalen van uitgegeven voertuigen: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Frontoffice,Backoffice")]
        public async Task<IActionResult> RegisterInname([FromForm] voertuigDto dto)
        {
            try
            {
                
                await _voertuigService.UpdateVoertuigStatusAsync(dto.VoertuigId, "Beschikbaar");

               
                if (!string.IsNullOrEmpty(dto.Beschrijving) && dto.Foto != null)
                {
                    await _schademeldingService.AddSchademeldingAsync(new Schademelding
                    {
                        VoertuigID = dto.VoertuigId,
                        KlantID = dto.KlantId,
                        Beschrijving = dto.Beschrijving,
                        Opmerkingen = dto.Opmerkingen,
                        FotoPath = dto.Foto != null ? await _schademeldingService.UploadFotoAsync(dto.Foto) : null,
                        Melddatum = DateTime.Now,
                        Status = "In Behandeling"
                    });
                }

                return Ok("Voertuiginname succesvol geregistreerd.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Fout bij het registreren van de inname: {ex.Message}");
            }
        }
    }


}
