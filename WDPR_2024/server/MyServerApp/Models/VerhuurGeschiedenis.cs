using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDPR_2024.server.MyServerApp.Models
{
    public class VerhuurGeschiedenis
    {
        public int VerhuurGeschiedenisID { get; set; }
        public int KlantID { get; set; }
        public Klant Klant { get; set; }
        public int VoertuigID { get; set; }
        public Voertuig Voertuig { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public decimal TotaleKosten { get; set; }
    }

}
