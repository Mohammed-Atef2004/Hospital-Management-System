using Application.Features.Appointments.DTOs;
using MediatR;

namespace Application.Features.Appointments.Queries.GetCancelledAppointments;

public record GetCancelledAppointmentsQuery
    : IRequest<IEnumerable<AppointmentResponse>>;