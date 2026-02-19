using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.RemoveWeeklyShift
{
    public class RemoveDoctorWeeklyShiftCommandHandler : IRequestHandler<RemoveDoctorWeeklyShiftCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RemoveDoctorWeeklyShiftCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(RemoveDoctorWeeklyShiftCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.Doctor.GetByIdAsync(request.DoctorId);
            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }
            var schedule = doctor.Schedules.FirstOrDefault(s => s.Id == request.ScheduleId);
            if (schedule == null)
            {
                throw new NotFoundException("Schedule not found");
            }
            doctor.Schedules.Remove(schedule);
            _unitOfWork.Doctor.Update(doctor);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
