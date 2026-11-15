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
        return new DoctorName(firstName.Trim(), lastName.Trim());
    }
}
