using Application.Common.Exceptions;
using Domain.personnel;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Shifts.Commands.UpdateShift
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

            var employee = await _unitOfWork.Staff.GetByIdWithShiftsAsync(request.staffId, cancellationToken);

            if (employee == null)
                throw new NotFoundException(nameof(Staff), request.staffId);

            employee.UpdateShift(request.shiftId, request.start, request.end);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
