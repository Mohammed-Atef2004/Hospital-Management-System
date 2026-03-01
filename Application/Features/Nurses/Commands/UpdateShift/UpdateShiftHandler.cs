using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Nurses.Commands.UpdateShift
{
    public class UpdateShiftHandler :
        IRequestHandler<UpdateShiftCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateShiftHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateShiftCommand request, CancellationToken cancellationToken)
        {
            var nurse = await _unitOfWork.Nurse.GetFirstOrDefaultAsync(n => n.Id == request.NurseId,
                include: q => q.Include(n => n.Schedules));
            if (nurse == null)
                throw new NotFoundException($"Nurse with id {request.NurseId} not found.");

            nurse.UpdateSchedule(request.ShiftId, request.day, request.start, request.end);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }


}
