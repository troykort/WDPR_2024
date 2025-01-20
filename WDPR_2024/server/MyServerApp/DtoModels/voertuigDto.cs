namespace WDPR_2024.server.MyServerApp.DtoModels
{
    public class voertuigDto
    {
        public int VoertuigId { get; set; }
        public int KlantId { get; set; } 
        public string? Beschrijving { get; set; }
        public string? Opmerkingen { get; set; }
        public IFormFile? Foto { get; set; }
    }

}
