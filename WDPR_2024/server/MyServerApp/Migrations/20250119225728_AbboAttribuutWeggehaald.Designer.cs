﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WDPR_2024.server.MyServerApp.Data;

#nullable disable

namespace MyServerApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250119225728_AbboAttribuutWeggehaald")]
    partial class AbboAttribuutWeggehaald
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Klant", b =>
                {
                    b.Property<int>("KlantID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KlantID"));

                    b.Property<string>("Adres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("BedrijfID")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool?>("IsActief")
                        .HasColumnType("bit");

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RijbewijsFotoPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RijbewijsNummer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefoonnummer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Wachtwoord")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("KlantID");

                    b.HasIndex("BedrijfID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserID")
                        .IsUnique();

                    b.ToTable("Klanten");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.Abonnement", b =>
                {
                    b.Property<int>("AbonnementID")
                        .HasColumnType("int");

                    b.Property<int?>("AantalHuurdagenPerJaar")
                        .HasColumnType("int");

                    b.Property<DateTime>("EindDatum")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("KortingOpVoertuighuur")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Kosten")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("KostenPerJaar")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("MaandelijkseAbonnementskosten")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("MaxVoertuigenPerMedewerker")
                        .HasColumnType("int");

                    b.Property<decimal?>("OvergebruikKostenPerDag")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("StartDatum")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AbonnementID");

                    b.ToTable("Abonnementen");
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.Bedrijf", b =>
                {
                    b.Property<int>("BedrijfID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BedrijfID"));

                    b.Property<int>("AbonnementID")
                        .HasColumnType("int");

                    b.Property<string>("Adres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Bedrijfsnaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailDomein")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("KVKNummer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BedrijfID");

                    b.HasIndex("EmailDomein")
                        .IsUnique();

                    b.ToTable("Bedrijven");
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.Medewerker", b =>
                {
                    b.Property<int>("MedewerkerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MedewerkerID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MedewerkerID");

                    b.HasIndex("UserID");

                    b.ToTable("Medewerkers");
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.Notificatie", b =>
                {
                    b.Property<int>("NotificatieID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificatieID"));

                    b.Property<string>("Bericht")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Gelezen")
                        .HasColumnType("bit");

                    b.Property<string>("Titel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("VerzondenOp")
                        .HasColumnType("datetime2");

                    b.Property<string>("userID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("NotificatieID");

                    b.HasIndex("userID");

                    b.ToTable("Notificaties");
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.Opmerking", b =>
                {
                    b.Property<int>("OpmerkingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OpmerkingID"));

                    b.Property<DateTime>("DatumToegevoegd")
                        .HasColumnType("datetime2");

                    b.Property<string>("GebruikerNaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tekst")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VerhuurAanvraagID")
                        .HasColumnType("int");

                    b.HasKey("OpmerkingID");

                    b.HasIndex("VerhuurAanvraagID");

                    b.ToTable("Opmerkingen");
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.Schademelding", b =>
                {
                    b.Property<int>("SchademeldingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SchademeldingID"));

                    b.Property<string>("Beschrijving")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FotoPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("KlantID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Melddatum")
                        .HasColumnType("datetime2");

                    b.Property<string>("Opmerkingen")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("VerhuurAanvraagID")
                        .HasColumnType("int");

                    b.Property<int>("VoertuigID")
                        .HasColumnType("int");

                    b.HasKey("SchademeldingID");

                    b.HasIndex("KlantID");

                    b.HasIndex("VerhuurAanvraagID");

                    b.HasIndex("VoertuigID");

                    b.ToTable("Schademeldingen");
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.VerhuurAanvraag", b =>
                {
                    b.Property<int>("VerhuurAanvraagID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VerhuurAanvraagID"));

                    b.Property<int?>("BedrijfID")
                        .HasColumnType("int");

                    b.Property<DateTime>("EindDatum")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FrontofficeMedewerkerID")
                        .HasColumnType("int");

                    b.Property<int>("KlantID")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDatum")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Uitgiftedatum")
                        .HasColumnType("datetime2");

                    b.Property<int>("VoertuigID")
                        .HasColumnType("int");

                    b.HasKey("VerhuurAanvraagID");

                    b.HasIndex("BedrijfID");

                    b.HasIndex("FrontofficeMedewerkerID");

                    b.HasIndex("KlantID");

                    b.HasIndex("VoertuigID");

                    b.ToTable("VerhuurAanvragen");
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.Voertuig", b =>
                {
                    b.Property<int>("VoertuigID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VoertuigID"));

                    b.Property<string>("Aanschafjaar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HuidigeHuurderEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("HuidigeHuurderID")
                        .HasColumnType("int");

                    b.Property<string>("HuidigeHuurderNaam")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Kenteken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Kleur")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Merk")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PrijsPerDag")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypeVoertuig")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Uitgiftedatum")
                        .HasColumnType("datetime2");

                    b.HasKey("VoertuigID");

                    b.ToTable("Voertuigen");
                });

            modelBuilder.Entity("Klant", b =>
                {
                    b.HasOne("WDPR_2024.server.MyServerApp.Models.Bedrijf", "Bedrijf")
                        .WithMany("Medewerkers")
                        .HasForeignKey("BedrijfID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("WDPR_2024.server.MyServerApp.Models.ApplicationUser", "User")
                        .WithOne()
                        .HasForeignKey("Klant", "UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bedrijf");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WDPR_2024.server.MyServerApp.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WDPR_2024.server.MyServerApp.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WDPR_2024.server.MyServerApp.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("WDPR_2024.server.MyServerApp.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.Abonnement", b =>
                {
                    b.HasOne("WDPR_2024.server.MyServerApp.Models.Bedrijf", "Bedrijf")
                        .WithOne("Abonnement")
                        .HasForeignKey("WDPR_2024.server.MyServerApp.Models.Abonnement", "AbonnementID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bedrijf");
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.Medewerker", b =>
                {
                    b.HasOne("WDPR_2024.server.MyServerApp.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.Notificatie", b =>
                {
                    b.HasOne("WDPR_2024.server.MyServerApp.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("userID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.Opmerking", b =>
                {
                    b.HasOne("WDPR_2024.server.MyServerApp.Models.VerhuurAanvraag", "VerhuurAanvraag")
                        .WithMany("Opmerkingen")
                        .HasForeignKey("VerhuurAanvraagID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("VerhuurAanvraag");
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.Schademelding", b =>
                {
                    b.HasOne("Klant", "Klant")
                        .WithMany("Schademeldingen")
                        .HasForeignKey("KlantID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("WDPR_2024.server.MyServerApp.Models.VerhuurAanvraag", "VerhuurAanvraag")
                        .WithMany()
                        .HasForeignKey("VerhuurAanvraagID");

                    b.HasOne("WDPR_2024.server.MyServerApp.Models.Voertuig", "Voertuig")
                        .WithMany("Schademeldingen")
                        .HasForeignKey("VoertuigID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Klant");

                    b.Navigation("VerhuurAanvraag");

                    b.Navigation("Voertuig");
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.VerhuurAanvraag", b =>
                {
                    b.HasOne("WDPR_2024.server.MyServerApp.Models.Bedrijf", "Bedrijf")
                        .WithMany("VerhuurAanvragen")
                        .HasForeignKey("BedrijfID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("WDPR_2024.server.MyServerApp.Models.Medewerker", "FrontofficeMedewerker")
                        .WithMany("BehandeldeAanvragen")
                        .HasForeignKey("FrontofficeMedewerkerID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Klant", "Klant")
                        .WithMany("VerhuurAanvragen")
                        .HasForeignKey("KlantID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("WDPR_2024.server.MyServerApp.Models.Voertuig", "Voertuig")
                        .WithMany("VerhuurAanvragen")
                        .HasForeignKey("VoertuigID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Bedrijf");

                    b.Navigation("FrontofficeMedewerker");

                    b.Navigation("Klant");

                    b.Navigation("Voertuig");
                });

            modelBuilder.Entity("Klant", b =>
                {
                    b.Navigation("Schademeldingen");

                    b.Navigation("VerhuurAanvragen");
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.Bedrijf", b =>
                {
                    b.Navigation("Abonnement")
                        .IsRequired();

                    b.Navigation("Medewerkers");

                    b.Navigation("VerhuurAanvragen");
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.Medewerker", b =>
                {
                    b.Navigation("BehandeldeAanvragen");
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.VerhuurAanvraag", b =>
                {
                    b.Navigation("Opmerkingen");
                });

            modelBuilder.Entity("WDPR_2024.server.MyServerApp.Models.Voertuig", b =>
                {
                    b.Navigation("Schademeldingen");

                    b.Navigation("VerhuurAanvragen");
                });
#pragma warning restore 612, 618
        }
    }
}
