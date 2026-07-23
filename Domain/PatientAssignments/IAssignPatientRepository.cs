using Domain.Entities;
using Domain.Repositories;

namespace Domain.PatientAssignments
{
    public interface IAssignPatientRepository : IGenericRepository<PatientAssignment>
    {
        public Task<bool> IsPatientAlreadyAssignedToNurseInShiftAsync(int patientId, int nurseId, int shiftId, CancellationToken cancellationToken);

        public Task<List<Patient>> GetPatientsByShiftAndNurseAsync(int shiftId, int nurseId, CancellationToken cancellationToken);
    }
}
