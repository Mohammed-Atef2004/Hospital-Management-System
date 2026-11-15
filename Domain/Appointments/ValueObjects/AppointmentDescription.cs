namespace Domain.Appointments.ValueObjects;

public sealed record AppointmentDescription
{
    public string Value { get; }
    private AppointmentDescription(string value) => Value = value;

    public static AppointmentDescription Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Trim().Length > 500)
            throw new ArgumentException("Appointment description must contain 1 to 500 characters.", nameof(value));
        return new AppointmentDescription(value.Trim());
    }
}
