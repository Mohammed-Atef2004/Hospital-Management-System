namespace Application.Features.Appointments.DTOs;

public sealed record AppointmentResponse(
    int Id,
    DateTime AppointmentDate,
    bool IsCancelled,
    string Description,
    int PatientId,
    int DoctorId);