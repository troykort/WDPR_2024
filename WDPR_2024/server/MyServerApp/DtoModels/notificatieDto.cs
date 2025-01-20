using WDPR_2024.server.MyServerApp.Models;

namespace WDPR_2024.server.MyServerApp.DtoModels
{
    public class notificatieDto
    {
        public int NotificatieID { get; set; }
        public int KlantID { get; set; }
        public int MedewerkerID { get; set; }
        public string Bericht { get; set; }

        public string Titel { get; set; }
        public DateTime VerzondenOp { get; set; }
        public bool Gelezen { get; set; }
    }

    
}
