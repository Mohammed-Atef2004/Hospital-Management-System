using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Authentication;

public sealed class CryptographicRefreshTokenService : IRefreshTokenService
{
    private readonly int _lifetimeDays;

    public CryptographicRefreshTokenService(int lifetimeDays)
    {
        if (lifetimeDays is < 1 or > 365) throw new ArgumentOutOfRangeException(nameof(lifetimeDays));
        _lifetimeDays = lifetimeDays;
    }

    public RefreshTokenIssue Create()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        var token = Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        return new RefreshTokenIssue(token, Hash(token), DateTime.UtcNow.AddDays(_lifetimeDays));
    }

    public string Hash(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
    }
}