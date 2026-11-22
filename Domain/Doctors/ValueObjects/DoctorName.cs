namespace Domain.Doctors.ValueObjects;

public sealed record DoctorName
{
    public string FirstName { get; }
    public string LastName { get; }
    public string DisplayName => $"{FirstName} {LastName}";

    private DoctorName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static DoctorName Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new Exceptions.InvalidDoctorNameException();
        if (string.IsNullOrWhiteSpace(lastName))
            throw new Exceptions.InvalidDoctorNameException();
        var normalizedFirstName = firstName.Trim();
        var normalizedLastName = lastName.Trim();
        if (normalizedFirstName.Length > 50 || normalizedLastName.Length > 50)
            throw new Exceptions.InvalidDoctorNameException("Doctor names cannot exceed 50 characters.");
        return new DoctorName(normalizedFirstName, normalizedLastName);
    }
}
