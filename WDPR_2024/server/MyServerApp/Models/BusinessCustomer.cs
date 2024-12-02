namespace backend.Models
{
    public class BusinessCustomer : CustomerBase
    {
        public string CompanyName { get; set; }
        public string KVK { get; set; }
        public string SubscriptionType { get; set; } // Bijvoorbeeld "Pay-as-you-go" of "Prepaid"
        public string Domain { get; set; } // Domein van het bedrijf, bijvoorbeeld "bedrijf.nl"
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
