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
        public decimal Kosten { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public string Status { get; set; } // "In Behandeling", "Actief", "Afgekeurd"
        public int? MaxVoertuigenPerMedewerker { get; set; } // Limiet per medewerker

        // Relatie naar Bedrijf
        public Bedrijf Bedrijf { get; set; }
    }

}
