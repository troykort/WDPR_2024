using System.Net;

namespace backend.Models
{
    public class CustomerBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; } 
    }
}
