using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WDPR_2024.server.MyServerApp.Models;

namespace WDPR_2024.server.MyServerApp.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser> // IdentityDbContext voor gebruikersbeheer
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets voor de verschillende modellen
        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Bedrijf> Bedrijven { get; set; }
        public DbSet<Abonnement> Abonnementen { get; set; }
        public DbSet<VerhuurAanvraag> VerhuurAanvragen { get; set; }
        public DbSet<Voertuig> Voertuigen { get; set; }
        public DbSet<Schademelding> Schademeldingen { get; set; }
        public DbSet<Notificatie> Notificaties { get; set; }
        public DbSet<Medewerker> Medewerkers { get; set; }
        public DbSet<Opmerking> Opmerkingen { get; set; }

        // Configuratie van de modellen
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Klant en Bedrijf Relatie
            modelBuilder.Entity<Klant>()
                .HasOne(k => k.Bedrijf)
                .WithMany(b => b.Medewerkers)
                .HasForeignKey(k => k.BedrijfID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Klant>()
    .HasOne(k => k.User)
    .WithOne()
    .HasForeignKey<Klant>(k => k.UserID)
    .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Opmerking>()
                .HasOne(o => o.VerhuurAanvraag)
                .WithMany(v => v.Opmerkingen)
                .HasForeignKey(o => o.VerhuurAanvraagID)
                .OnDelete(DeleteBehavior.NoAction);

            // Relatie tussen Medewerker en ApplicationUser
            modelBuilder.Entity<Medewerker>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserID);

            

            // Bedrijf en Abonnement Relatie
            modelBuilder.Entity<Bedrijf>()
                .HasOne(b => b.Abonnement)
                .WithOne(a => a.Bedrijf)
                .HasForeignKey<Abonnement>(a => a.AbonnementID)
                .OnDelete(DeleteBehavior.Cascade);

            // VerhuurAanvraag en Klant Relatie
            modelBuilder.Entity<VerhuurAanvraag>()
                .HasOne(v => v.Klant)
                .WithMany(k => k.VerhuurAanvragen)
                .HasForeignKey(v => v.KlantID)
                .OnDelete(DeleteBehavior.NoAction);

            // VerhuurAanvraag en Voertuig Relatie
            modelBuilder.Entity<VerhuurAanvraag>()
                .HasOne(v => v.Voertuig)
                .WithMany(vh => vh.VerhuurAanvragen)
                .HasForeignKey(v => v.VoertuigID)
                .OnDelete(DeleteBehavior.NoAction);

            // VerhuurAanvraag en Bedrijf Relatie
            modelBuilder.Entity<VerhuurAanvraag>()
                .HasOne(v => v.Bedrijf)
                .WithMany(b => b.VerhuurAanvragen)
                .HasForeignKey(v => v.BedrijfID)
                .OnDelete(DeleteBehavior.Restrict);

            // VerhuurAanvraag en Medewerker Relatie
            modelBuilder.Entity<VerhuurAanvraag>()
                .HasOne(v => v.FrontofficeMedewerker)
                .WithMany(m => m.BehandeldeAanvragen)
                .HasForeignKey(v => v.FrontofficeMedewerkerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Schademelding en Klant Relatie
            modelBuilder.Entity<Schademelding>()
                .HasOne(s => s.Klant)
                .WithMany(k => k.Schademeldingen)
                .HasForeignKey(s => s.KlantID)
                .OnDelete(DeleteBehavior.NoAction);

            // Schademelding en Voertuig Relatie
            modelBuilder.Entity<Schademelding>()
                .HasOne(s => s.Voertuig)
                .WithMany(v => v.Schademeldingen)
                .HasForeignKey(s => s.VoertuigID)
                .OnDelete(DeleteBehavior.NoAction);

            

            // Configuratie voor unieke velden (bijvoorbeeld email)
            modelBuilder.Entity<Klant>()
                .HasIndex(k => k.Email)
                .IsUnique();

            modelBuilder.Entity<Bedrijf>()
                .HasIndex(b => b.EmailDomein)
                .IsUnique();
        }
    }
}
