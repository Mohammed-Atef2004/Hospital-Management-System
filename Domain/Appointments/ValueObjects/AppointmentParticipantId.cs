namespace Domain.Appointments.ValueObjects;

public sealed record AppointmentParticipantId
{
    public int Value { get; }
    private AppointmentParticipantId(int value) => Value = value;

    public static AppointmentParticipantId Create(int value, string parameterName)
    {
        if (value <= 0) throw new ArgumentOutOfRangeException(parameterName, "ID must be greater than zero.");
        return new AppointmentParticipantId(value);
    }
}
