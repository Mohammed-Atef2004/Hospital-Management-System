using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var doctor = await _unitOfWork.Doctor.GetFirstOrDefaultAsync(
                d => d.Id == request.DoctorId,
                include: q => q.Include(d => d.Schedules));
            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }
            doctor.RemoveSchedule(request.ScheduleId);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
