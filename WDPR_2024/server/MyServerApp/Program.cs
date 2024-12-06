using WDPR_2024.server.MyServerApp.Models;
using WDPR_2024.server.MyServerApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using WDPR_2024.server.MyServerApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WDPR_2024.server.MyServerApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            // Configureer Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

            // alle andere services 
            builder.Services.AddScoped<VoertuigService>();
            builder.Services.AddScoped<BedrijfService>();
            builder.Services.AddScoped<VerhuurAanvraagService>();
            builder.Services.AddScoped<KlantService>();
            builder.Services.AddScoped<AbonnementService>();
            builder.Services.AddScoped<SchademeldingService>();
            builder.Services.AddScoped<NotificatieService>();

            // Voeg EmailService toe (bijvoorbeeld voor SMTP)
            //builder.Services.AddSingleton(new EmailService(
            //    smtpServer: "smtp.example.com", // SMTP server zoals smtp.gmail.com
            //    smtpPort: 587, // Meestal 587 voor TLS, of 465 voor SSL
            //    smtpUser: "your-email@example.com", // Gebruik jouw e-mailadres
            //    smtpPassword: "your-email-password" // Het wachtwoord voor je SMTP-server
            //));

            // Configure JWT authentication
            var key = Encoding.ASCII.GetBytes("your_very_long_secret_key_here_that_is_at_least_32_bytes_long");
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "your_issuer",
                    ValidAudience = "your_audience",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            var app = builder.Build();

            // Middleware configureren
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Map de controllers naar de juiste endpoints
            app.MapControllers(); 

            // Seed the database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                SeedData.Initialize(services).Wait();
            }

            app.Run();
        }
    }
}
