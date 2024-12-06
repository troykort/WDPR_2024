using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WDPR_2024.server.MyServerApp.Models
{
    public class Bedrijf
    {
        public int BedrijfID { get; set; }
        public string Bedrijfsnaam { get; set; }
        public string Adres { get; set; }
        public string KVKNummer { get; set; }
        public string EmailDomein { get; set; }
        public string ContactEmail { get; set; }

     
        public int AbonnementID { get; set; }
        public Abonnement Abonnement { get; set; }

        public ICollection<VerhuurAanvraag> VerhuurAanvragen { get; set; }
        public ICollection<Klant> Medewerkers { get; set; }
    }


}
