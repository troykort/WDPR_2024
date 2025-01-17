namespace WDPR_2024.server.MyServerApp.DtoModels
{
   public class VerhuurGeschiedenisDto
{
    public int VerhuurAanvraagID { get; set; }
    public string KlantNaam { get; set; }
    public string VoertuigInfo { get; set; }
    public DateTime StartDatum { get; set; }
    public DateTime EindDatum { get; set; }
    // public decimal TotaleKosten { get; set; }
}
}
