using Domain.SharedKernel;

namespace Domain.Appointments.Events;

public sealed record AppointmentBookedDomainEvent(int PatientId, int DoctorId, DateTime ScheduledFor) : DomainEvent;
public sealed record AppointmentCancelledDomainEvent(int AppointmentId, int PatientId, int DoctorId, DateTime ScheduledFor) : DomainEvent;
public sealed record AppointmentRescheduledDomainEvent(int AppointmentId, DateTime PreviousDate, DateTime NewDate) : DomainEvent;
public sealed record AppointmentDescriptionChangedDomainEvent(int AppointmentId, string PreviousDescription, string NewDescription) : DomainEvent;
public sealed record AppointmentDoctorChangedDomainEvent(int AppointmentId, int PreviousDoctorId, int NewDoctorId) : DomainEvent;
public sealed record AppointmentPatientChangedDomainEvent(int AppointmentId, int PreviousPatientId, int NewPatientId) : DomainEvent;
