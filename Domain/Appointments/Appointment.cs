using Domain.Doctors;

namespace Domain.Entities;

public class Appointment : CommonData
{
    public int Id { get; private set; }

    public DateTime AppointmentDate { get; private set; }

    public bool IsCancelled { get; private set; }

    public string Description { get; private set; }

    public int PatientId { get; private set; }
    public Patient Patient { get; private set; }

    public int DoctorId { get; private set; }
    public Doctor Doctor { get; private set; }

    private Appointment()
    {
    }

    private Appointment(
        DateTime appointmentDate,
        string description,
        int patientId,
        int doctorId)
    {
        ValidateAppointmentDate(appointmentDate);
        ValidateDescription(description);
        ValidateId(patientId, nameof(patientId));
        ValidateId(doctorId, nameof(doctorId));

        AppointmentDate = appointmentDate;
        Description = description;
        PatientId = patientId;
        DoctorId = doctorId;
        IsCancelled = false;
    }

    public static Appointment Create(
        DateTime appointmentDate,
        string description,
        int patientId,
        int doctorId)
    {
        return new Appointment(
            appointmentDate,
            description,
            patientId,
            doctorId);
    }

    public void Cancel()
    {
        EnsureNotCancelled();

        IsCancelled = true;
    }

    public void ChangeDescription(string description)
    {
        EnsureNotCancelled();
        ValidateDescription(description);

        Description = description;
    }

    public void ChangeDoctor(int doctorId)
    {
        EnsureNotCancelled();
        ValidateId(doctorId, nameof(doctorId));

        DoctorId = doctorId;
    }

    public void ChangePatient(int patientId)
    {
        EnsureNotCancelled();
        ValidateId(patientId, nameof(patientId));

        PatientId = patientId;
    }

    public void ChangeAppointmentDate(DateTime appointmentDate)
    {
        EnsureNotCancelled();
        ValidateAppointmentDate(appointmentDate);

        AppointmentDate = appointmentDate;
    }

    private void EnsureNotCancelled()
    {
        if (IsCancelled)
        {
            throw new InvalidOperationException(
                "A cancelled appointment cannot be modified.");
        }
    }

    private static void ValidateAppointmentDate(DateTime appointmentDate)
    {
        if (appointmentDate <= DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Appointment date must be in the future.",
                nameof(appointmentDate));
        }
    }

    private static void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException(
                "Appointment description cannot be empty.",
                nameof(description));
        }
    }

    private static void ValidateId(int id, string parameterName)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(
                parameterName,
                "ID must be greater than zero.");
        }
    }
}