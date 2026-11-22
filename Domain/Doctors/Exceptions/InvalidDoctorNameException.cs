namespace Domain.Doctors.Exceptions
{
    [Serializable]
    public class InvalidDoctorNameException : Domain.SharedKernel.DomainException
    {
       
        public InvalidDoctorNameException(string? message = null)
            : base(message ?? "First name and last name are required.")
        {
        }
       
    }
}
