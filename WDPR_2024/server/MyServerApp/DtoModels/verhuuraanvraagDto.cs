namespace WDPR_2024.server.MyServerApp.DtoModels
{
    public class verhuurAanvraagDto
    {
        public int VerhuurAanvraagID { get; set; }
        public string KlantNaam { get; set; }
        public string VoertuigInfo { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public string Status { get; set; }


    }

    public class UpdateStatusRequest
    {
        public string userID { get; set; }
        public string Opmerkingen { get; set; }
    }

    

}
