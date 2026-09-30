using Application.Features.Appointments.DTOs;
using MediatR;

namespace Application.Features.Appointments.Queries.GetAllAppointments;

public record GetAllAppointmentsQuery
    : IRequest<IEnumerable<AppointmentResponse>>;