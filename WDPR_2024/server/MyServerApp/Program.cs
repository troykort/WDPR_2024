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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    policy.WithOrigins("http://localhost:3000/") 
                          .AllowAnyHeader()    
                          .AllowAnyMethod()   
                          .AllowCredentials(); 
                });
            });

            
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

         
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

            
            builder.Services.AddScoped<VoertuigService>();
            builder.Services.AddScoped<BedrijfService>();
            builder.Services.AddScoped<VerhuurAanvraagService>();
            builder.Services.AddScoped<KlantService>();
            builder.Services.AddScoped<AbonnementService>();
            builder.Services.AddScoped<SchademeldingService>();
            builder.Services.AddScoped<NotificatieService>();
            builder.Services.AddScoped<MedewerkerService>();



            //Email Service wordt not later toegevoegd
             builder.Services.AddSingleton(new EmailService(
                 smtpServer: "smtp.example.com", 
                 smtpPort: 587, 
                 smtpUser: "your-email@example.com", 
                 smtpPassword: "your-email-password" 
             ));

            // Configure JWT authentication
            var key = Encoding.ASCII.GetBytes("your_very_long_secret_key_here_32_bytes!");
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
            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();



            // Map de controllers naar de juiste endpoints

            app.UseCors("AllowSpecificOrigins");

            

            app.MapControllers(); // Alleen API endpoints

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


