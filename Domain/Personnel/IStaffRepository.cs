using Domain.Repositories;

namespace Domain.personnel
{
    public interface IStaffRepository : IGenericRepository<Staff>
    {
        Task<Staff?> GetByIdWithShiftsAsync(int id, CancellationToken cancellationToken);
    }
}
