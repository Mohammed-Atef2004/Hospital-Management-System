using Application.Features.Appointments.Commands.Cancel;
using Domain.Interfaces;
using Domain.Repositories;
using MediatR;

public class CancelAppointmentCommandHandler
    : IRequestHandler<CancelAppointmentCommand, Unit>
{
    private readonly IAppointmentRepository _appointmentsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelAppointmentCommandHandler(
        IAppointmentRepository appointmentsRepository,
        IUnitOfWork unitOfWork)
    {
        _appointmentsRepository = appointmentsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        CancelAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        var appointment =
            await _appointmentsRepository.GetFirstOrDefaultAsync(
                x => x.Id == request.Id);

        if (appointment is null)
            throw new ArgumentException(
                $"Appointment with Id {request.Id} was not found.");

        appointment.Cancel();

        _appointmentsRepository.Update(appointment);

        await _unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}