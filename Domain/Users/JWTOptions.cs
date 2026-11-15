namespace Domain.Users;

public sealed class JWTOptions
{
    public const string SectionName = "JWT";

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int LifetimeSeconds { get; set; } = 3600;
    public string SigningKey { get; set; } = string.Empty;
}
