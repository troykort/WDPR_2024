namespace WDPR_2024.server.MyServerApp.DtoModels
{
    public class SchadeRequest
    {
        public int KlantID { get; set; }
        public int VoertuigID { get; set; }
        public string Beschrijving { get; set; }
        public string? FotoPath { get; set; }
        public string? Opmerkingen { get; set; }
    }

}
