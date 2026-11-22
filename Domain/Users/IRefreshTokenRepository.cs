using Domain.Repositories;

namespace Domain.Users;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default);
    Task RevokeAllForUserAsync(Guid userId, DateTime utcNow, CancellationToken cancellationToken = default);
}
