using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDPR_2024.server.MyServerApp.Models
{
    public class Abonnement
    {
        public int AbonnementID { get; set; }
        public string Type { get; set; } // "Pay-as-you-go", "Prepaid"
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public string Status { get; set; } // "In Behandeling", "Actief", "Afgekeurd"
        public int? MaxVoertuigenPerMedewerker { get; set; } = 1; // Limiet per medewerker

        // Pay-as-you-go specific properties
        public decimal? MaandelijkseAbonnementskosten { get; set; }
        public decimal? KortingOpVoertuighuur { get; set; }

        // Prepaid specific properties
        public int? AantalHuurdagenPerJaar { get; set; }
        public decimal? KostenPerJaar { get; set; }
        public decimal? OvergebruikKostenPerDag { get; set; }

        public Bedrijf Bedrijf { get; set; }
    }
}
