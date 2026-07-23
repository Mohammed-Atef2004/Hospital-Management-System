using Application.Common.Exceptions;
using Domain.Interfaces.Services;
using Domain.Nurses;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Nurses.Commands.UpdateNurse
{
    public class UpdateNurseHandler :
        IRequestHandler<UpdateNurseCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateNurseHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }


        public async Task<Unit> Handle(UpdateNurseCommand request, CancellationToken cancellationToken)
        {

            var currentUserId = _currentUserService.UserId
                ?? throw new UnauthorizedAccessException("User must be logged in.");

            var nurse = await _unitOfWork.Nurse.GetByIdAsync(currentUserId)
                ?? throw new NotFoundException(nameof(Nurse), currentUserId);

            var currentUser = await _unitOfWork.Nurse.GetByIdAsync(currentUserId);

            nurse.Update(request.Address, request.City, request.PhoneNumber);

            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
