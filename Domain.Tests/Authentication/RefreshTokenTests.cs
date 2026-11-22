using Application.Features.Authentication;
using Domain.Users;

namespace Domain.Tests.Authentication;

public sealed class RefreshTokenTests
{
    [Fact]
    public void CryptographicService_ShouldIssueOpaqueTokensAndStableHashes()
    {
        var service = new CryptographicRefreshTokenService(30);
        var issued = service.Create();

        Assert.NotEqual(issued.Token, issued.TokenHash);
        Assert.Equal(64, issued.TokenHash.Length);
        Assert.Equal(issued.TokenHash, service.Hash(issued.Token));
        Assert.InRange(issued.ExpiresAtUtc, DateTime.UtcNow.AddDays(29), DateTime.UtcNow.AddDays(31));
    }

    [Fact]
    public void RefreshToken_ShouldBecomeUnusableAfterRotationOrExpiry()
    {
        var token = RefreshToken.Issue(Guid.NewGuid(), new string('A', 64), DateTime.UtcNow.AddDays(1));
        Assert.True(token.IsUsable(DateTime.UtcNow));

        token.Revoke(DateTime.UtcNow, new string('B', 64));

        Assert.False(token.IsUsable(DateTime.UtcNow));
        Assert.Equal(new string('B', 64), token.ReplacedByTokenHash);
    }
}
