using Application.Features.Appointments.DTOs;
using MediatR;

namespace Application.Features.Appointments.Queries.GetPatientAppointments;

public record GetPatientAppointmentsQuery(int PatientId)
    : IRequest<IEnumerable<AppointmentResponse>>;