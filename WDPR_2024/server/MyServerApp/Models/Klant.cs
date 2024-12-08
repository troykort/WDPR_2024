using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDPR_2024.server.MyServerApp.Models
{
    public class Klant
    {
        public int KlantID { get; set; }
        public string Naam { get; set; }
        public string Adres { get; set; }
        public string Email { get; set; }
        public string Telefoonnummer { get; set; }
        public string Wachtwoord { get; set; }
        public string? RijbewijsNummer { get; set; }
        public string? RijbewijsFotoPath { get; set; } // Voor geüploade rijbewijsfoto
        public bool? IsActief { get; set; }

        // Relaties
        public int? BedrijfID { get; set; }
        public Bedrijf? Bedrijf { get; set; }
        public ICollection<VerhuurAanvraag>? VerhuurAanvragen { get; set; }
        public ICollection<Schademelding>? Schademeldingen { get; set; }

        public ICollection<Notificatie>? Notificaties { get; set; }
    }

}
