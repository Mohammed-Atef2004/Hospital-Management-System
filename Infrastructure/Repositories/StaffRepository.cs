using Domain.Nurses;
using Domain.personnel;
using Infastructure.Presistence.Database;
using Infastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class StaffRepository : GenericRepository<Staff>, IStaffRepository
    {
        public StaffRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Staff?> GetByIdWithShiftsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Set<Staff>()
                .AsSplitQuery()
                .Include(s => s.Shifts)
                .Include(s => (s as Nurse).PatientAssignments)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }
    }
}

