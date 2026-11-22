using Application.Features.Appointments.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Application.Common.Exceptions;

namespace Application.Features.Appointments.Queries.GetAppointmentById;

public class GetAppointmentByIdQueryHandler
    : IRequestHandler<GetAppointmentByIdQuery, AppointmentResponse>
{
    private readonly IAppointmentRepository _appointmentsRepository;
    private readonly IMapper _mapper;

    public GetAppointmentByIdQueryHandler(
        IAppointmentRepository appointmentsRepository,
        IMapper mapper)
    {
        _appointmentsRepository = appointmentsRepository;
        _mapper = mapper;
    }

    public async Task<AppointmentResponse> Handle(
        GetAppointmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var appointment =
            await _appointmentsRepository
                .GetFirstOrDefaultAsync(x => x.Id == request.Id);

        if (appointment == null)
        {
            throw new NotFoundException("Appointment", request.Id);
        }

        return _mapper.Map<AppointmentResponse>(appointment);
    }
}
