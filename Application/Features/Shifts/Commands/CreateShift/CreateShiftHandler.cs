using Application.Common.Exceptions;
using Domain.personnel;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Shifts.Commands.CreateShift
{
    public class CreateShiftHandler :
        IRequestHandler<CreateShiftCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;


        public CreateShiftHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public async Task<int> Handle(CreateShiftCommand request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Staff.GetByIdAsync(request.StaffId);

            if (employee == null)
                throw new NotFoundException(nameof(Staff), request.StaffId);

            var shift = employee.AddShift(request.start, request.end);

            await _unitOfWork.CompleteAsync();
            return shift.Id;
        }
    }
}