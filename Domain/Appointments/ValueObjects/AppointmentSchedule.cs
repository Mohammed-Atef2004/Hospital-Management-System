namespace Domain.Appointments.ValueObjects;

using Domain.SharedKernel;

public sealed record AppointmentSchedule
{
    public DateTime Value { get; }
    private AppointmentSchedule(DateTime value) => Value = value;

    public static AppointmentSchedule Create(DateTime value)
    {
        if (value <= DateTime.UtcNow)
            throw new DomainException("Appointment date must be in the future.");
        return new AppointmentSchedule(value);
    }
}
