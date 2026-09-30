using Application.Features.Appointments.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Appointments.Queries.GetPatientAppointments;

public class GetPatientAppointmentsQueryHandler
    : IRequestHandler<
        GetPatientAppointmentsQuery,
        IEnumerable<AppointmentResponse>>
{
    private readonly IAppointmentRepository _appointmentsRepository;
    private readonly IMapper _mapper;

    public GetPatientAppointmentsQueryHandler(
        IAppointmentRepository appointmentsRepository,
        IMapper mapper)
    {
        _appointmentsRepository = appointmentsRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentResponse>> Handle(
        GetPatientAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var appointments =
            await _appointmentsRepository
                .GetAllAsync(x => x.PatientId == request.PatientId);

        return _mapper.Map<IEnumerable<AppointmentResponse>>(
            appointments);
    }
}