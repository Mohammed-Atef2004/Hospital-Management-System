using Application.Features.Authentication;

namespace Domain.Tests.Authentication;

public sealed class TotpCodeServiceTests
{
    [Fact]
    public void VerifyCode_ShouldMatchTheRfc6238Sha1TestVector()
    {
        var service = new TotpCodeService();
        const string secret = "GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQ";

        Assert.True(service.VerifyCode(secret, "287082", DateTimeOffset.FromUnixTimeSeconds(59)));
        Assert.False(service.VerifyCode(secret, "287083", DateTimeOffset.FromUnixTimeSeconds(59)));
    }

    [Fact]
    public void GenerateSecret_ShouldReturnBase32SecretAcceptedByVerifier()
    {
        var service = new TotpCodeService();
        var secret = service.GenerateSecret();

        Assert.Equal(32, secret.Length);
        Assert.All(secret, character => Assert.Contains(character, "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567"));
        Assert.False(service.VerifyCode(secret, "not-code", DateTimeOffset.UtcNow));
    }
}
