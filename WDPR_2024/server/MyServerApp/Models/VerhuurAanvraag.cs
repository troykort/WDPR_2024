using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WDPR_2024.server.MyServerApp.Models
{
    public class VerhuurAanvraag
    {
        public int VerhuurAanvraagID { get; set; }
        public int KlantID { get; set; }
        public Klant? Klant { get; set; }
        public int VoertuigID { get; set; }
        public Voertuig? Voertuig { get; set; }
        public int? BedrijfID { get; set; } 
        public Bedrijf? Bedrijf { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public string? Status { get; set; } // "In Behandeling", "Goedgekeurd", "Afgekeurd", "Uitgegeven"
        public ICollection<Opmerking> Opmerkingen { get; set; } = new List<Opmerking>();
        public DateTime? Uitgiftedatum { get; set; } 
        public int? FrontofficeMedewerkerID { get; set; } // Medewerker die de uitgifte registreert
        public Medewerker? FrontofficeMedewerker { get; set; }
    }


}
