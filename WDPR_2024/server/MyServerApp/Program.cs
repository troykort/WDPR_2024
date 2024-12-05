using WDPR_2024.server.MyServerApp.Models;
using WDPR_2024.server.MyServerApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using WDPR_2024.server.MyServerApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<YourDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<YourDbContext>()
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
builder.Services.AddSingleton(new EmailService(
smtpServer: "smtp.example.com", // Bijvoorbeeld: smtp.gmail.com
smtpPort: 587, // Meestal 587 voor TLS of 465 voor SSL
smtpUser: "your-email@example.com", // Jouw e-mailadres
smtpPassword: "your-email-password" // Wachtwoord voor de SMTP-server
)); // twijfel situatie maar we komen er wel later uit. 



var app = builder.Build();

// Middleware
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // Alleen API endpoints

app.Run();
