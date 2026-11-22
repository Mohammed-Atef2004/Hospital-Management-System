using Domain.SharedKernel;

namespace Domain.Users;

public sealed class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime? RevokedAtUtc { get; private set; }
    public string? ReplacedByTokenHash { get; private set; }
    public byte[] RowVersion { get; private set; } = null!;

    private RefreshToken() { }

    private RefreshToken(Guid userId, string tokenHash, DateTime expiresAtUtc)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        TokenHash = tokenHash;
        CreatedAtUtc = DateTime.UtcNow;
        ExpiresAtUtc = DateTime.SpecifyKind(expiresAtUtc, DateTimeKind.Utc);
    }

    public static RefreshToken Issue(Guid userId, string tokenHash, DateTime expiresAtUtc)
    {
        if (userId == Guid.Empty) throw new DomainException("A refresh token must belong to an account.");
        if (string.IsNullOrWhiteSpace(tokenHash) || tokenHash.Length != 64)
            throw new DomainException("A refresh token must be stored as a SHA-256 hash.");
        if (expiresAtUtc <= DateTime.UtcNow)
            throw new DomainException("A refresh token must expire in the future.");
        return new RefreshToken(userId, tokenHash, expiresAtUtc);
    }

    public bool IsUsable(DateTime utcNow)
        => RevokedAtUtc is null && ExpiresAtUtc > utcNow;

    public void Revoke(DateTime utcNow, string? replacementTokenHash = null)
    {
        if (RevokedAtUtc.HasValue) return;
        RevokedAtUtc = DateTime.SpecifyKind(utcNow, DateTimeKind.Utc);
        ReplacedByTokenHash = replacementTokenHash;
    }
}
