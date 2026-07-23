using Domain.Schedules;
using Domain.Shifts;
using Infastructure.Presistence.Database;
using Infastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ShiftRepository
           : GenericRepository<Shift>, IShiftRepository
    {
        public ShiftRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<Shift>> GetShiftsByStaffIdAsync(int staffId, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Where(s => s.StaffId == staffId && s.IsActive)
                .OrderByDescending(s => s.StartDateTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Shift>> GetShiftsInTimeRangeAsync(
    int staffId, DateTime startDateTime,
    DateTime endDateTime, CancellationToken cancellationToken)
        {
            return await _context.Set<Shift>()
                .AsNoTracking()
                .Where(s => s.StaffId == staffId
                         && !s.IsDeleted
                         && s.IsActive
                         && s.StartDateTime < endDateTime
                         && s.EndDateTime > startDateTime)
                .OrderBy(s => s.StartDateTime)
                .ToListAsync(cancellationToken);
        }
    }
}
