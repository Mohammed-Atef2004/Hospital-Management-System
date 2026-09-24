using Domain.Entities;
using MediatR;

namespace Application.Features.Appointments.Commands.Create;

public record CreateAppointmentCommand(
    DateTime AppointmentDate,
    string Description,
    int PatientId,
    int DoctorId) : IRequest<Appointment>;