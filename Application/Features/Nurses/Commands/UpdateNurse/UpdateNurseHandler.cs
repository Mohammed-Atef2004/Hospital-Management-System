using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Nurses.Commands.UpdateNurse
{
    public class UpdateNurseHandler :
        IRequestHandler<UpdateNurseCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateNurseHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<Unit> Handle(UpdateNurseCommand request, CancellationToken cancellationToken)
        {
            var nurse = await _unitOfWork.Nurse.GetByIdAsync(request.NurseId);
            if (nurse == null)
                throw new NotFoundException($"Nurse with id {request.NurseId} not found.");

            var departmentExists =
                await _unitOfWork.Department
                    .GetByIdAsync(request.DeparId);

            if (departmentExists == null)
                throw new NotFoundException($"Department with id {request.DeparId} not found.");

            nurse.Update(request.Address, request.City, request.PhoneNumber, request.DeparId);

            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
