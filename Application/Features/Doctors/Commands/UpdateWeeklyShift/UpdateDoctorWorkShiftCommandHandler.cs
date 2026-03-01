using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Doctors.Commands.UpdateWeeklyShift
{
    public class UpdateDoctorWorkShiftCommandHandler : IRequestHandler<UpdateDoctorWorkShiftCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateDoctorWorkShiftCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateDoctorWorkShiftCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.Doctor.GetFirstOrDefaultAsync(
                d => d.Id == request.DoctorId,
                include: q => q.Include(d => d.Schedules));
            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }
            doctor.UpdateSchedule(request.ScheduleId, request.NewDay, request.NewStart, request.NewEnd);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
