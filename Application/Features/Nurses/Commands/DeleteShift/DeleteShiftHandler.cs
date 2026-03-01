using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Nurses.Commands.DeleteShift
{
    public class DeleteShiftHandler
     : IRequestHandler<DeleteShiftCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteShiftHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteShiftCommand request, CancellationToken cancellationToken)
        {
            var nurse = await _unitOfWork.Nurse.GetFirstOrDefaultAsync(n => n.Id == request.NurseId,
                include: q => q.Include(n => n.Schedules));

            if (nurse == null)
                throw new NotFoundException($"Nurse with id {request.NurseId} not found.");

            nurse.RemoveSchedule(request.ShiftId);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
