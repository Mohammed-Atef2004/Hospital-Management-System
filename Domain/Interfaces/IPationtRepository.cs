using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        public void Update(Patient patient);
        public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    }
}
