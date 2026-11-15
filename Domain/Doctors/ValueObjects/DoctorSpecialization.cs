namespace Domain.Doctors.ValueObjects;

public sealed record DoctorSpecialization
{
    public string Value { get; }

    private DoctorSpecialization(string value) => Value = value;

    public static DoctorSpecialization Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new Exceptions.InvalidSpecializationException();
        return new DoctorSpecialization(value.Trim());
    }
}
