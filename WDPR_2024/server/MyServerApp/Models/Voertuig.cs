using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDPR_2024.server.MyServerApp.Models
{
    public class Voertuig
    {
        public int VoertuigID { get; set; }
        public string Merk { get; set; }
        public string Type { get; set; } // Auto, Camper, Caravan
        public string Kenteken { get; set; }
        public string Kleur { get; set; }
        public string Aanschafjaar { get; set; }
        public bool Beschikbaar { get; set; }
        public decimal PrijsPerDag { get; set; }
        public string Status { get; set; } // "Beschikbaar", "Onderhoud", "Verhuurd"
        public string Opmerkingen { get; set; } // Notities over onderhoud of conditie

        // Relaties
        public ICollection<VerhuurAanvraag> VerhuurAanvragen { get; set; }
        public ICollection<Schademelding> Schademeldingen { get; set; }
    }

}
