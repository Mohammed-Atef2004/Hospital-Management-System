using Application.Features.Appointments.DTOs;
using MediatR;

namespace Application.Features.Appointments.Queries.GetAppointmentById;

public sealed record GetAppointmentByIdQuery(int Id)
    : IRequest<AppointmentResponse>;