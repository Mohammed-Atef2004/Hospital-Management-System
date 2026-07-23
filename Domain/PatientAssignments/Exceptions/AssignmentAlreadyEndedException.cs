using Domain.SharedKernel;

namespace Domain.PatientAssignments.Exceptions
{
    public sealed class AssignmentAlreadyEndedException : DomainException
    {
        public AssignmentAlreadyEndedException()
            : base("This assignment has already ended.")
        {
        }
    }
}
