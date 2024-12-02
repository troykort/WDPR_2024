namespace backend.Utilities
{
    public static class EmailValidator
    {
        public static bool IsValidEmployeeEmail(string email, string domain)
        {
            var emailDomain = email.Split('@').Last();
            return emailDomain.Equals(domain, StringComparison.OrdinalIgnoreCase);
        }
    }
}
