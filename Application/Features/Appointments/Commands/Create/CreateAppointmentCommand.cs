using Application.Common;
using Domain.Entities;
using MediatR;

namespace Application.Features.Appointments.Commands.Create;

public record CreateAppointmentCommand(
    DateTime AppointmentDate,
    string Description,
    int PatientId,
    int DoctorId) : ICommandRequest<Appointment>;