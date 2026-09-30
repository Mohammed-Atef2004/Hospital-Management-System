using Application.Features.Appointments.DTOs;
using MediatR;

namespace Application.Features.Appointments.Queries.GetDoctorAppointments;

public record GetDoctorAppointmentsQuery(int DoctorId)
    : IRequest<IEnumerable<AppointmentResponse>>;