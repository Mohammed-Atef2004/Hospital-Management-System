using Application.Features.Appointments.DTOs;
using MediatR;

namespace Application.Features.Appointments.Queries.GetUpcomingAppointments;

public record GetUpcomingAppointmentsQuery
    : IRequest<IEnumerable<AppointmentResponse>>;