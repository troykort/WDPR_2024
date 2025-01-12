namespace WDPR_2024.server.MyServerApp.Models
{
    public class Opmerking
    {
        public int OpmerkingID { get; set; }
        public string Tekst { get; set; } // De inhoud van de opmerking
        public int VerhuurAanvraagID { get; set; } // Verwijzing naar de verhuuraanvraag
        public VerhuurAanvraag VerhuurAanvraag { get; set; }
        public string GebruikerNaam { get; set; } // Naam van de gebruiker die de opmerking heeft toegevoegd
        public DateTime DatumToegevoegd { get; set; } // Datum van de opmerking
    }

}
