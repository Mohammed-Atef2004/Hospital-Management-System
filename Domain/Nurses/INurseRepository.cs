using Domain.Repositories;

namespace Domain.Nurses
{
    public interface INurseRepository : IGenericRepository<Nurse>
    {
        public Task<Nurse?> GetByIdWithShiftsAndAssignmentsAsync(int id, CancellationToken cancellationToken);
    }
}
