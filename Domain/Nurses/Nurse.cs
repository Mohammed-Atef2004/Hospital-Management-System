using Domain.Departments;
using Domain.PatientAssignments;
using Domain.personnel;
using Domain.SharedKernel;

namespace Domain.Nurses
{
    public class Nurse : Staff
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string PhoneNumber { get; private set; }
        public int? DepartmentId { get; private set; }
        public Department? Department { get; private set; }
        public bool IsHeadNurse { get; private set; }
        protected readonly List<PatientAssignment> _patientAssignments = new();
        public IReadOnlyCollection<PatientAssignment> PatientAssignments => _patientAssignments.AsReadOnly();


        private Nurse() { }

        private Nurse(string firstName, string lastName, string address, string city, string? phoneNumber, bool isHead)
        : base()
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            City = city;
            PhoneNumber = phoneNumber;
            IsHeadNurse = isHead;
        }

        public static Nurse Register(string firstName, string lastName, string address, string city, string? phoneNumber, bool isHead)
        {
            return new Nurse(firstName, lastName, address, city, phoneNumber, isHead);
        }

        public void Update(string? address, string? city, string? phoneNumber)
        {
            Address = address;
            City = city;
            PhoneNumber = phoneNumber;
            UpdatedAt = DateTime.UtcNow;

        }

        public override void RemoveShift(int shiftId)
        {

            var activeAssignments = _patientAssignments
                .Where(a => a.ShiftId == shiftId && a.IsActive)
                .ToList();

            foreach (var assignment in activeAssignments)
            {
                assignment.EndAssignment();
            }
            base.RemoveShift(shiftId);
        }

        public PatientAssignment AssignPatient(int patientId, int shiftId, int assignedById)
        {
            var shift = _shifts.FirstOrDefault(s => s.Id == shiftId && !s.IsDeleted && s.IsActive);
            if (shift == null)
                throw new DomainException("Cannot assign patient to an inactive or non-existent shift for this nurse.");

            var isAlreadyAssigned = _patientAssignments.Any(a =>
                a.PatientId == patientId &&
                a.ShiftId == shiftId &&
                a.IsActive);

            if (isAlreadyAssigned)
                throw new DomainException("This patient is already actively assigned to this nurse in the specified shift.");

            var assignment = PatientAssignment.Create(patientId, Id, assignedById, shiftId);
            _patientAssignments.Add(assignment);

            return assignment;
        }

        public void UpdateDepartment(int? departmentId)
        {
            DepartmentId = departmentId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeHeadNurseStatus(bool isHeadNurse)
        {
            IsHeadNurse = isHeadNurse;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
