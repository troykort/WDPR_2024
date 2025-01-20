using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WDPR_2024.server.MyServerApp.Models;
using WDPR_2024.server.MyServerApp.Services;

namespace WDPR_2024.server.MyServerApp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly KlantService _klantService;
        private readonly MedewerkerService _medewerkerService;
        private readonly BedrijfService _bedrijfService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            KlantService klantService,
            MedewerkerService medewerkerService,
            BedrijfService bedrijfService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _klantService = klantService;
            _medewerkerService = medewerkerService;
            _bedrijfService = bedrijfService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();

                int? klantId = null;
                int? medewerkerId = null;

                if (role == "Particulier" || role == "Zakelijk")
                {
                    var klant = await _klantService.GetKlantByUserIdAsync(user.Id);
                    klantId = klant?.KlantID;
                }
                else
                {
                    var medewerker = await _medewerkerService.GetMedewerkerByUserIdAsync(user.Id);
                    medewerkerId = medewerker?.MedewerkerID;
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, role ?? "Particulier"),
                    new Claim("UserID", user.Id),
                    new Claim("KlantID", klantId?.ToString() ?? string.Empty),
                    new Claim("MedewerkerID", medewerkerId?.ToString() ?? string.Empty)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("your_very_long_secret_key_here_32_bytes!");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = "your_issuer",
                    Audience = "your_audience"
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new
                {
                    Token = tokenString,
                    KlantID = klantId,
                    MedewerkerID = medewerkerId,
                    UserID = user.Id
                });
            }
            return Unauthorized("Invalid credentials.");
        }

        [HttpPost("register/particulier")]
        public async Task<IActionResult> RegisterParticulier([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Particulier");

                var nieuweKlant = new Klant
                {
                    Naam = model.Naam,
                    Adres = model.Adres,
                    Email = model.Email,
                    Telefoonnummer = model.Telefoonnummer,
                    Wachtwoord = model.Password,
                    IsActief = true,
                    UserID = user.Id,
                    Rol = "Particulier"
                };

                try
                {
                    await _klantService.AddKlantAsync(nieuweKlant);
                }
                catch (Exception ex)
                {
                   
                    await _userManager.DeleteAsync(user);
                    return BadRequest($"Failed to save klant: {ex.Message}");
                }

                return Ok(new { UserId = user.Id, KlantID = nieuweKlant.KlantID });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("register/zakelijk")]
        public async Task<IActionResult> RegisterZakelijk([FromBody] RegisterZakelijkModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = model.ContactEmail,
                Email = model.ContactEmail,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Zakelijk");

                var abonnement = new Abonnement
                {
                    Type = model.AbonnementType,
                    MaandelijkseAbonnementskosten = model.MaandelijkseAbonnementskosten,
                    KortingOpVoertuighuur = model.KortingOpVoertuighuur,
                    AantalHuurdagenPerJaar = model.AantalHuurdagenPerJaar,
                    KostenPerJaar = model.KostenPerJaar,
                    OvergebruikKostenPerDag = model.OvergebruikKostenPerDag,
                    StartDatum = DateTime.UtcNow,
                    EindDatum = DateTime.UtcNow.AddYears(1),
                    Status = "Actief"
                };

                var bedrijf = new Bedrijf
                {
                    Bedrijfsnaam = model.Bedrijfsnaam,
                    Adres = model.Adres,
                    KVKNummer = model.KVKNummer,
                    EmailDomein = model.EmailDomein,
                    ContactEmail = model.ContactEmail,
                    Abonnement = abonnement
                };

                try
                {
                    await _bedrijfService.AddBedrijfAsync(bedrijf);
                }
                catch (Exception ex)
                {
                    await _userManager.DeleteAsync(user);
                    return BadRequest($"Failed to save bedrijf: {ex.Message}");
                }

                return Ok(new { UserId = user.Id, BedrijfID = bedrijf.BedrijfID });
            }

            return BadRequest(result.Errors);
        }

    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public string Naam { get; set; }
        public string Adres { get; set; }
        public string Email { get; set; }
        public string Telefoonnummer { get; set; }
        public string Password { get; set; }
    }

    public class RegisterZakelijkModel
    {
        public string Bedrijfsnaam { get; set; }
        public string Adres { get; set; }
        public string KVKNummer { get; set; }
        public string EmailDomein { get; set; }
        public string ContactEmail { get; set; }
        public string Password { get; set; }

        public string AbonnementType { get; set; } // "Pay-as-you-go" of "Prepaid"
        public decimal? MaandelijkseAbonnementskosten { get; set; }
        public decimal? KortingOpVoertuighuur { get; set; }
        public int? AantalHuurdagenPerJaar { get; set; }
        public decimal? KostenPerJaar { get; set; }
        public decimal? OvergebruikKostenPerDag { get; set; }
    }
}
