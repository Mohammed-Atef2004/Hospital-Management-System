namespace Application.Features.Authentication;

public sealed record AuthResponse(
    string AccessToken,
    DateTime ExpiresAtUtc,
    Guid UserId,
    string Username,
    string Role,
    string RefreshToken,
    DateTime RefreshTokenExpiresAtUtc);
