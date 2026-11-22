using Domain.Entities;
using Domain.Interfaces;
using Domain.Repositories;
using Domain.SharedKernel;
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
        var doctor = await _unitOfWork.Doctor.GetBookingAvailabilityAsync(request.DoctorId, cancellationToken);
        if (doctor is null)
            throw new Application.Common.Exceptions.NotFoundException("Active doctor", request.DoctorId);
        if (!doctor.IsAvailableForAppointment(request.AppointmentDate))
            throw new DomainException("The doctor is not available at the requested appointment time.");
        if (!await _unitOfWork.Patient.ExistsAsync(patient => patient.Id == request.PatientId, cancellationToken))
            throw new Application.Common.Exceptions.NotFoundException("Patient", request.PatientId);

        var appointment = Appointment.Create(
            request.AppointmentDate,
            request.Description,
            request.PatientId,
            request.DoctorId);

        await _appointmentsRepository.AddAsync(appointment);

        await _unitOfWork.CompleteAsync();

        return appointment;
    }
}
