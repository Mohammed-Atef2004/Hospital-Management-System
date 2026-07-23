
using Domain.Entities;
using Domain.PatientAssignments;
using Infastructure.Presistence.Database;
using Infastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AssignPatientRepository : GenericRepository<PatientAssignment>, IAssignPatientRepository
    {
        public AssignPatientRepository(ApplicationDbContext context) : base(context) { }
        public async Task<bool> IsPatientAlreadyAssignedToNurseInShiftAsync(int patientId, int nurseId, int shiftId, CancellationToken cancellationToken)
        {
            return await _dbSet
                .AnyAsync(pa =>
                    pa.PatientId == patientId &&
                    pa.NurseId == nurseId &&
                    pa.ShiftId == shiftId &&
                    pa.IsActive,
                    cancellationToken);
        }

        public async Task<List<Patient>> GetPatientsByShiftAndNurseAsync(int shiftId, int nurseId, CancellationToken cancellationToken)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(a => a.ShiftId == shiftId && a.NurseId == nurseId && a.IsActive)
                .Include(a => a.Patient)
                .Select(a => a.Patient)
                .ToListAsync(cancellationToken);
        }
    }
}
