namespace Domain.Appointments.ValueObjects;

public sealed record AppointmentSchedule
{
    public DateTime Value { get; }
    private AppointmentSchedule(DateTime value) => Value = value;

    public static AppointmentSchedule Create(DateTime value)
    {
        if (value <= DateTime.UtcNow)
            throw new ArgumentException("Appointment date must be in the future.", nameof(value));
        return new AppointmentSchedule(value);
    }
}
