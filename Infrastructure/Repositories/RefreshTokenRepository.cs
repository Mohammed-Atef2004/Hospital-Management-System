using Domain.Users;
using Infastructure.Presistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Repositories;

public sealed class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context) { }

    public Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default)
        => _dbSet.SingleOrDefaultAsync(token => token.TokenHash == tokenHash, cancellationToken);

    public async Task RevokeAllForUserAsync(Guid userId, DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var tokens = await _dbSet.Where(token => token.UserId == userId && token.RevokedAtUtc == null)
            .ToListAsync(cancellationToken);
        foreach (var token in tokens)
            token.Revoke(utcNow);
    }
}
