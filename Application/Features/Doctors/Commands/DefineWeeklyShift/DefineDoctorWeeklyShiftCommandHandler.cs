using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.DefineWeeklyShift
{
    public class DefineDoctorWeeklyShiftCommandHandler : IRequestHandler<DefineDoctorWeeklyShiftCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DefineDoctorWeeklyShiftCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DefineDoctorWeeklyShiftCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.Doctor.GetByIdAsync(request.DoctorId);
            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }
            var schedule = new Schedule
            {
                DayofWeek = request.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                DoctorId = request.DoctorId
            };
            doctor.Schedules.Add(schedule);
            _unitOfWork.Doctor.Update(doctor);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
