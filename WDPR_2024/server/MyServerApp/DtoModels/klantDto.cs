namespace WDPR_2024.server.MyServerApp.DtoModels
{
    public class KlantDto
    {
        public int KlantID { get; set; }
        public string Email { get; set; } // For login and response
        public string Password { get; set; } // For login only
    }
}
