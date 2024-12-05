using Microsoft.AspNetCore.Identity;

namespace WDPR_2024.server.MyServerApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }

    }
}
