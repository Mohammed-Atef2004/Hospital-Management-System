using Domain.Nurses;
using Infastructure.Presistence.Database;
using Infastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class NurseRepository
       : GenericRepository<Nurse>, INurseRepository
    {
        public NurseRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Nurse?> GetByIdWithShiftsAndAssignmentsAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbSet
                .AsSplitQuery()
                .Include(s => s.Shifts)
                .Include(s => s.PatientAssignments)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }
    }
}
