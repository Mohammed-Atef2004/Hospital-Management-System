using Domain.Interfaces;
using Domain.Repositories;
using Domain.SharedKernel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Appointments.Commands.UpdateAppointmentDate
{
    public class UpdateAppointmentDateCommandHandler : IRequestHandler<UpdateAppointmentDateCommand>
    {
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAppointmentDateCommandHandler(IAppointmentRepository appointmentsRepository, IUnitOfWork unitOfWork)
        {
            _appointmentsRepository = appointmentsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateAppointmentDateCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentsRepository.GetFirstOrDefaultAsync(x => x.Id == request.Id);
            if (appointment == null)
                throw new Application.Common.Exceptions.NotFoundException("Appointment", request.Id);
            var doctor = await _unitOfWork.Doctor.GetBookingAvailabilityAsync(appointment.DoctorId, cancellationToken);
            if (doctor is null || !doctor.IsAvailableForAppointment(request.AppointmentDate, appointment.Id))
                throw new DomainException("The doctor is not available at the requested appointment time.");
            appointment.ChangeAppointmentDate(request.AppointmentDate);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
