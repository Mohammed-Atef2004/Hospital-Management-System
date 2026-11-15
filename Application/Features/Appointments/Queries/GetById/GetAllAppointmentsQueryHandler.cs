using Application.Features.Appointments.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Appointments.Queries.GetAllAppointments;

public class GetAllAppointmentsQueryHandler
    : IRequestHandler<
        GetAllAppointmentsQuery,
        IEnumerable<AppointmentResponse>>
{
    private readonly IAppointmentRepository _appointmentsRepository;
    private readonly IMapper _mapper;

    public GetAllAppointmentsQueryHandler(
        IAppointmentRepository appointmentsRepository,
        IMapper mapper)
    {
        _appointmentsRepository = appointmentsRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentResponse>> Handle(
        GetAllAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var appointments =
            await _appointmentsRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<AppointmentResponse>>(
            appointments);
    }
}