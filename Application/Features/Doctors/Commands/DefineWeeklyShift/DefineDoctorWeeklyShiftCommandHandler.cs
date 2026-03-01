using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var doctor = await _unitOfWork.Doctor.GetFirstOrDefaultAsync(
                d => d.Id == request.DoctorId,
                include: q => q.Include(d => d.Schedules));

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            doctor.AddSchedule(request.DayOfWeek, request.StartTime, request.EndTime);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
