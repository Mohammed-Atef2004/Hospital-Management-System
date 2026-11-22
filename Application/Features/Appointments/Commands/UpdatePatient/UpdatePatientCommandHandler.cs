using Domain.Interfaces;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Appointments.Commands.UpdatePatient
{
    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand>
    {
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePatientCommandHandler(IAppointmentRepository appointmentsRepository, IUnitOfWork unitOfWork)
        {
            _appointmentsRepository = appointmentsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentsRepository.GetFirstOrDefaultAsync(x => x.Id == request.Id);
            if (appointment == null)
                throw new Application.Common.Exceptions.NotFoundException("Appointment", request.Id);
            if (!await _unitOfWork.Patient.ExistsAsync(patient => patient.Id == request.PatientId, cancellationToken))
                throw new Application.Common.Exceptions.NotFoundException("Patient", request.PatientId);
            appointment.ChangePatient(request.PatientId);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
