﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly BedrijfService _bedrijfService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            KlantService klantService,
            BedrijfService bedrijfService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _klantService = klantService;
            _bedrijfService = bedrijfService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "Particulier"),
                    new Claim("Id", user.Id) // Add User ID for easier backend reference
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

                return Ok(new { Token = tokenString });
            }
            return Unauthorized("Invalid credentials.");
        }

        [HttpPost("register/particulier")]
        public async Task<IActionResult> RegisterParticulier([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var nieuweKlant = new Klant
                {
                    Naam = model.Naam,
                    Adres = model.Adres,
                    Email = model.Email,
                    Telefoonnummer = model.Telefoonnummer,
                    Wachtwoord = model.Password
                };

                await _klantService.AddKlantAsync(nieuweKlant);

                // Retrieve the ID of the created user from the database
                var createdUser = await _userManager.FindByEmailAsync(nieuweKlant.Email);
                return Ok(new { UserId = createdUser.Id });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error during registration: {ex.Message}");
            }
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
                await _userManager.AddToRoleAsync(user, "Abonnementbeheerder");

                // Add company to the database
                var bedrijf = new Bedrijf
                {
                    Bedrijfsnaam = model.Bedrijfsnaam,
                    Adres = model.Adres,
                    KVKNummer = model.KVKNummer,
                    EmailDomein = model.EmailDomein,
                    ContactEmail = model.ContactEmail
                };
                await _bedrijfService.AddBedrijfAsync(bedrijf);

                return Ok(new { user.Id });
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
    }
}
