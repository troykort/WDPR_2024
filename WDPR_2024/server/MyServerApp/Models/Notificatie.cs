using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WDPR_2024.server.MyServerApp.Models
{
    public class Notificatie
    {
        public int NotificatieID { get; set; }
        public int KlantID { get; set; }
        public Klant Klant { get; set; }
        public string Bericht { get; set; }
        public DateTime VerzondenOp { get; set; }
        public bool Gelezen { get; set; }
    }

}
