using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDPR_2024.server.MyServerApp.Models
{
    public class Medewerker
    {
        public int MedewerkerID { get; set; }
        public string Naam { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; } // "Backoffice", "Frontoffice"

        // Relaties
        public ICollection<VerhuurAanvraag> BehandeldeAanvragen { get; set; }
    }

}
