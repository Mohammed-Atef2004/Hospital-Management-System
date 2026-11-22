using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<Patient?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
