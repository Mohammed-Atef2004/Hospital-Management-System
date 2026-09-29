using Domain.Interfaces;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Appointments.Commands.UpdateDoctor
{
    public class UpdatePatientCommandHandler : IRequestHandler<UpdateDoctorCommand>
    {
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePatientCommandHandler(IAppointmentRepository appointmentsRepository, IUnitOfWork unitOfWork)
        {
            _appointmentsRepository = appointmentsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            var appointment = _appointmentsRepository.GetFirstOrDefaultAsync(x => x.Id == request.Id).Result;
            if (appointment == null)
            {
                throw new ArgumentException("No Appointment with this Id");
            }
            appointment.ChangeDoctor(request.DoctorId);
            return Unit.Value;
        }
    }
}
