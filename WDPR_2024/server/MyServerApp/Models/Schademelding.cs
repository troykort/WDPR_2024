using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WDPR_2024.server.MyServerApp.Models
{
    public class Schademelding
    {
        public int SchademeldingID { get; set; }
        public int VoertuigID { get; set; }
        public Voertuig Voertuig { get; set; }
        public int KlantID { get; set; }
        public Klant Klant { get; set; }
        public string Beschrijving { get; set; }
        public string FotoPath { get; set; } // Optioneel: Foto van de schade
        public DateTime Melddatum { get; set; }
        public string Status { get; set; } // "In Behandeling", "Afgehandeld"
    }

}
