using Domain.Entities;
using Domain.Interfaces;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Appointments.Commands.Create;

public class CreateAppointmentCommandHandler
    : IRequestHandler<CreateAppointmentCommand, Appointment>
{
    private readonly IAppointmentRepository _appointmentsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAppointmentCommandHandler(
        IAppointmentRepository appointmentsRepository,
        IUnitOfWork unitOfWork)
    {
        _appointmentsRepository = appointmentsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Appointment> Handle(
        CreateAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        var appointment = Appointment.Create(
            request.AppointmentDate,
            request.Description,
            request.PatientId,
            request.DoctorId);

        await _appointmentsRepository.AddAsync(
            appointment);

        await _unitOfWork.CompleteAsync();

        return appointment;
    }
}