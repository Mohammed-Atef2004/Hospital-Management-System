using Domain.SharedKernel;

namespace Domain.Doctors.Exceptions;

public sealed class InvalidDoctorOfficeException : DomainException
{
    public InvalidDoctorOfficeException(string message) : base(message) { }
}
