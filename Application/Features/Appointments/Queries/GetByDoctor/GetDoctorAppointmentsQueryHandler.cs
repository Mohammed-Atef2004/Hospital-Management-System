using Application.Features.Appointments.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Appointments.Queries.GetDoctorAppointments;

public class GetDoctorAppointmentsQueryHandler
    : IRequestHandler<
        GetDoctorAppointmentsQuery,
        IEnumerable<AppointmentResponse>>
{
    private readonly IAppointmentRepository _appointmentsRepository;
    private readonly IMapper _mapper;

    public GetDoctorAppointmentsQueryHandler(
        IAppointmentRepository appointmentsRepository,
        IMapper mapper)
    {
        _appointmentsRepository = appointmentsRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentResponse>> Handle(
        GetDoctorAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var appointments =
            await _appointmentsRepository
                .GetAllAsync(x => x.DoctorId == request.DoctorId);

        return _mapper.Map<IEnumerable<AppointmentResponse>>(
            appointments);
    }
}