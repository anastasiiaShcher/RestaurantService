namespace Dvor.Web.Infrastructure
{
    public class ApiAuthSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public int ExpirationTimeInSeconds { get; set; }
    }
}