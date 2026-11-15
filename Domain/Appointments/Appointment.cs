using Domain.Doctors;

namespace Domain.Entities;

using Domain.Appointments.Events;
using Domain.Appointments.ValueObjects;
using Domain.SharedKernel;
using System.ComponentModel.DataAnnotations.Schema;

public class Appointment : CommonData, IAggregateRoot
{
    public int Id { get; private set; }

    public DateTime AppointmentDate { get; private set; }

    public bool IsCancelled { get; private set; }

    public string Description { get; private set; }

    public int PatientId { get; private set; }
    public Patient Patient { get; private set; }

    public int DoctorId { get; private set; }
    public Doctor Doctor { get; private set; }

    private readonly List<DomainEvent> _domainEvents = new();
    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Appointment()
    {
    }

    private Appointment(
        DateTime appointmentDate,
        string description,
        int patientId,
        int doctorId)
    {
        AppointmentDate = AppointmentSchedule.Create(appointmentDate).Value;
        Description = AppointmentDescription.Create(description).Value;
        PatientId = AppointmentParticipantId.Create(patientId, nameof(patientId)).Value;
        DoctorId = AppointmentParticipantId.Create(doctorId, nameof(doctorId)).Value;
        IsCancelled = false;
    }

    public static Appointment Create(
        DateTime appointmentDate,
        string description,
        int patientId,
        int doctorId)
    {
        var appointment = new Appointment(
            appointmentDate,
            description,
            patientId,
            doctorId);
        appointment.AddDomainEvent(new AppointmentBookedDomainEvent(appointment.PatientId, appointment.DoctorId, appointment.AppointmentDate));
        return appointment;
    }

    public void Cancel()
    {
        EnsureNotCancelled();

        IsCancelled = true;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new AppointmentCancelledDomainEvent(Id, PatientId, DoctorId, AppointmentDate));
    }

    public void ChangeDescription(string description)
    {
        EnsureNotCancelled();
        var previousDescription = Description;
        Description = AppointmentDescription.Create(description).Value;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new AppointmentDescriptionChangedDomainEvent(Id, previousDescription, Description));
    }

    public void ChangeDoctor(int doctorId)
    {
        EnsureNotCancelled();
        var previousDoctorId = DoctorId;
        doctorId = AppointmentParticipantId.Create(doctorId, nameof(doctorId)).Value;
        DoctorId = doctorId;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new AppointmentDoctorChangedDomainEvent(Id, previousDoctorId, DoctorId));
    }

    public void ChangePatient(int patientId)
    {
        EnsureNotCancelled();
        var previousPatientId = PatientId;
        patientId = AppointmentParticipantId.Create(patientId, nameof(patientId)).Value;
        PatientId = patientId;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new AppointmentPatientChangedDomainEvent(Id, previousPatientId, PatientId));
    }

    public void ChangeAppointmentDate(DateTime appointmentDate)
    {
        EnsureNotCancelled();
        var previousDate = AppointmentDate;
        AppointmentDate = AppointmentSchedule.Create(appointmentDate).Value;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new AppointmentRescheduledDomainEvent(Id, previousDate, AppointmentDate));
    }

    private void EnsureNotCancelled()
    {
        if (IsCancelled)
        {
            throw new InvalidOperationException(
                "A cancelled appointment cannot be modified.");
        }
    }

    private void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
