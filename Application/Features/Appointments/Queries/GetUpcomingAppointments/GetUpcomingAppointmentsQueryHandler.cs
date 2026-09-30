using Application.Features.Appointments.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Appointments.Queries.GetUpcomingAppointments;

public class GetUpcomingAppointmentsQueryHandler
    : IRequestHandler<
        GetUpcomingAppointmentsQuery,
        IEnumerable<AppointmentResponse>>
{
    private readonly IAppointmentRepository _appointmentsRepository;
    private readonly IMapper _mapper;

    public GetUpcomingAppointmentsQueryHandler(
        IAppointmentRepository appointmentsRepository,
        IMapper mapper)
    {
        _appointmentsRepository = appointmentsRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentResponse>> Handle(
        GetUpcomingAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var appointments =
            await _appointmentsRepository.GetAllAsync(
                x =>
                    x.AppointmentDate > DateTime.UtcNow &&
                    !x.IsCancelled);

        return _mapper.Map<IEnumerable<AppointmentResponse>>(
            appointments);
    }
}