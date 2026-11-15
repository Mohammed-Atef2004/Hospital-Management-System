using Application.Features.Appointments.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Appointments.Queries.GetCancelledAppointments;

public class GetCancelledAppointmentsQueryHandler
    : IRequestHandler<
        GetCancelledAppointmentsQuery,
        IEnumerable<AppointmentResponse>>
{
    private readonly IAppointmentRepository _appointmentsRepository;
    private readonly IMapper _mapper;

    public GetCancelledAppointmentsQueryHandler(
        IAppointmentRepository appointmentsRepository,
        IMapper mapper)
    {
        _appointmentsRepository = appointmentsRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentResponse>> Handle(
        GetCancelledAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var appointments =
            await _appointmentsRepository
                .GetAllAsync(x => x.IsCancelled);

        return _mapper.Map<IEnumerable<AppointmentResponse>>(
            appointments);
    }
}