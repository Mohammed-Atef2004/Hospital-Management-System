using Domain.SharedKernel;

namespace Domain.Departments.Exceptions
{
    [Serializable]
    internal class DepartmentInvalidDescriptionException : DomainException
    {

        public DepartmentInvalidDescriptionException()
            : base($"Department is not valid") { }


    }
}