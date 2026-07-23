using Application.Common.Exceptions;
using Domain.personnel;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Shifts.Commands.RemoveShift
{
    public class RemoveShiftHandler :
        IRequestHandler<RemoveShiftCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;


        public RemoveShiftHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(RemoveShiftCommand request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Staff.GetByIdWithShiftsAsync(request.staffId, cancellationToken);

            if (employee == null)
                throw new NotFoundException(nameof(Staff), request.staffId);

            employee.RemoveShift(request.shiftId);

            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
