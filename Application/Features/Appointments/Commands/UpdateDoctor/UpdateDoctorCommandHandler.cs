using Domain.Interfaces;
using Domain.Repositories;
using Domain.SharedKernel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Appointments.Commands.UpdateDoctor
{
    public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand>
    {
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDoctorCommandHandler(IAppointmentRepository appointmentsRepository, IUnitOfWork unitOfWork)
        {
            _appointmentsRepository = appointmentsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentsRepository.GetFirstOrDefaultAsync(x => x.Id == request.Id);
            if (appointment == null)
                throw new Application.Common.Exceptions.NotFoundException("Appointment", request.Id);
            var doctor = await _unitOfWork.Doctor.GetBookingAvailabilityAsync(request.DoctorId, cancellationToken);
            if (doctor is null || !doctor.IsAvailableForAppointment(appointment.AppointmentDate, appointment.Id))
                throw new DomainException("The doctor is not available at the requested appointment time.");
            appointment.ChangeDoctor(request.DoctorId);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
