using Domain.Repositories;
using Domain.Shifts;

namespace Domain.Schedules
{
    public interface IShiftRepository
      : IGenericRepository<Shift>
    {
        public Task<List<Shift>> GetShiftsByStaffIdAsync(int staffId, CancellationToken cancellationToken);

        public Task<List<Shift>> GetShiftsInTimeRangeAsync(
    int staffId, DateTime startDateTime,
    DateTime endDateTime, CancellationToken cancellationToken);
    }
}


