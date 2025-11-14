namespace PL
{
    public class JWTOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string lifetime { get; set; }
        public string SigningKey { get; set; }
    }

}
