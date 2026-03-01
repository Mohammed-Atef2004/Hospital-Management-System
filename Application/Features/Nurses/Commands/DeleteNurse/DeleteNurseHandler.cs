using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Nurses.Commands.DeleteNurse
{
    internal class DeleteNurseHandler :
        IRequestHandler<DeleteNurseCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteNurseHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteNurseCommand request, CancellationToken cancellationToken)
        {
            var nurse = await _unitOfWork.Nurse.GetByIdAsync(request.NurseId);
            if (nurse == null)
                throw new NotFoundException($"Nurse with id {request.NurseId} not found.");

            nurse.SoftDelete();

            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
