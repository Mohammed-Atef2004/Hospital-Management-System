
using Domain.Entities;
using Domain.Nurses;
using Domain.SharedKernel;

namespace Domain.PatientAssignments
{
    public class PatientAssignment : CommonData
    {
        public int Id { get; private set; }
        public int PatientId { get; private set; }
        public Patient Patient { get; private set; } = null!;
        public int NurseId { get; private set; }
        public Nurse Nurse { get; private set; } = null!;
        public int AssignedBy { get; private set; }
        public int ShiftId { get; private set; }
        public DateTime AssignedAt { get; private set; }
        public DateTime? EndedAt { get; private set; }
        public bool IsActive { get; private set; } = true;
        private PatientAssignment() { }
        private PatientAssignment(
            int patientId,
            int nurseId,
            int assignedBy,
            int shiftId
            )
        {
            PatientId = patientId;
            NurseId = nurseId;
            CreatedAt = DateTime.UtcNow;
            AssignedBy = assignedBy;
            ShiftId = shiftId;
            AssignedAt = DateTime.UtcNow;
            IsActive = true;

        }

        public static PatientAssignment Create(
            int patientId,
            int nurseId,
            int assignedBy,
            int shiftId)
        {
            return new PatientAssignment(patientId, nurseId, assignedBy, shiftId);
        }
        public void EndAssignment()
        {
            if (!IsActive)
                throw new DomainException("This assignment has already ended.");
            EndedAt = DateTime.UtcNow;
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

    }
}
