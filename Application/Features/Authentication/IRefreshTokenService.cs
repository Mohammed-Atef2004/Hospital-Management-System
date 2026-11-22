namespace Application.Features.Authentication;

public interface IRefreshTokenService
{
    RefreshTokenIssue Create();
    string Hash(string token);
}

public sealed record RefreshTokenIssue(string Token, string TokenHash, DateTime ExpiresAtUtc);