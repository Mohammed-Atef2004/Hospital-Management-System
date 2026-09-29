using Domain.Interfaces;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Appointments.Commands.UpdateAppointmentDate
{
    public class UpdateDoctorCommandHandler : IRequestHandler<UpdatePatientCommand>
    {
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDoctorCommandHandler(IAppointmentRepository appointmentsRepository, IUnitOfWork unitOfWork)
        {
            _appointmentsRepository = appointmentsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var appointment = _appointmentsRepository.GetFirstOrDefaultAsync(x => x.Id == request.Id).Result;
            if (appointment == null)
            {
                throw new ArgumentException("No Appointment with this Id");
            }
            appointment.ChangeAppointmentDate(request.AppointmentDate);
            return Unit.Value;
        }
    }
}
