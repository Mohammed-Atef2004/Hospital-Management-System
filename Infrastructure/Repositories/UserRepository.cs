using Domain.Users;
using Domain.Users.ValueObjects;
using Infastructure.Presistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Repositories;

public sealed class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }

    public Task<bool> ExistsByEmailAsync(Email email, CancellationToken ct = default)
        => _dbSet.AnyAsync(user => user.Email.Value == email.Value && user.IsActive && !user.IsDeleted, ct);

    public Task<bool> ExistsByUsernameAsync(Username username, CancellationToken ct = default)
        => _dbSet.AnyAsync(user => user.Username.Value == username.Value && user.IsActive && !user.IsDeleted, ct);

    public Task<User?> GetByIdentityIdAsync(string identityId, CancellationToken ct = default)
        => _dbSet.Include(user => user.StaffProfile)
            .SingleOrDefaultAsync(user => user.IdentityId == identityId, ct);

    public async Task<IReadOnlyList<User>> GetByRoleAsync(UserRole role, CancellationToken ct = default)
        => await _dbSet.Where(user => user.Role == role && user.IsActive && !user.IsDeleted)
            .OrderBy(user => user.Username.Value)
            .ToListAsync(ct);

    public void SoftDelete(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        Update(user);
    }
}
