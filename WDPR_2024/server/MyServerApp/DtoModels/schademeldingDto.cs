namespace WDPR_2024.server.MyServerApp.DtoModels
{
    public class AddSchademeldingDto
    {
        public int VoertuigID { get; set; }
        public int KlantID { get; set; }
        public string Beschrijving { get; set; }
        public IFormFile? Foto { get; set; }
        public string? Opmerkingen { get; set; }
        public int? VerhuurAanvraagID { get; set; }
    }

    public class UpdateSchademeldingDto
    {
        public string Status { get; set; }
        public string? Opmerkingen { get; set; }
    }

}
