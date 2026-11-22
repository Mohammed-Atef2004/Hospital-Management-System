using Domain.Entities;
using Domain.Repositories;
using Infastructure.Presistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Repositories;

public sealed class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(ApplicationDbContext context) : base(context) { }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        => _dbSet.AnyAsync(patient => patient.Id == id && !patient.IsDeleted, cancellationToken);

    public Task<Patient?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => _dbSet.FirstOrDefaultAsync(patient => patient.UserId == userId && !patient.IsDeleted, cancellationToken);
}