using WDPR_2024.server.MyServerApp.Models;

public class Klant
{
    public int KlantID { get; set; }
    public string Naam { get; set; }
    public string Adres { get; set; }
    public string Email { get; set; }
    public string Telefoonnummer { get; set; }
    public string Wachtwoord { get; set; }
    public string UserID { get; set; } // Foreign Key naar ApplicationUser.Id
    public ApplicationUser? User { get; set; }
    public string? RijbewijsNummer { get; set; }
    public string? RijbewijsFotoPath { get; set; }
    public bool? IsActief { get; set; }
    public string Rol { get; set; } 

    // Relaties
    public int? BedrijfID { get; set; }
    public Bedrijf? Bedrijf { get; set; }
    public ICollection<VerhuurAanvraag>? VerhuurAanvragen { get; set; }
    public ICollection<Schademelding>? Schademeldingen { get; set; }
  
}
