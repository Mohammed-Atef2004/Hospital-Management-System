namespace Domain.Doctors.ValueObjects;

public sealed record DoctorSpecialization
{
    public string Value { get; }

    private DoctorSpecialization(string value) => Value = value;

    public static DoctorSpecialization Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new Exceptions.InvalidSpecializationException();
        var normalized = value.Trim();
        if (normalized.Length > 100)
            throw new Exceptions.InvalidSpecializationException("Specialization cannot exceed 100 characters.");
        return new DoctorSpecialization(normalized);
    }
}
