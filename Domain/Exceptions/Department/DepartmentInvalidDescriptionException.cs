namespace Domain.Exceptions.Department
{
    [Serializable]
    internal class DepartmentInvalidDescriptionException : DomainException
    {

        public DepartmentInvalidDescriptionException()
            : base($"Department is not valid") { }


    }
}