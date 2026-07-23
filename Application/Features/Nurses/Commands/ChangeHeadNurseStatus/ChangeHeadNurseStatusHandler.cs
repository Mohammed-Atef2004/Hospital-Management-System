using Application.Common.Exceptions;
using Domain.Interfaces.Services;
using Domain.Nurses;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Nurses.Commands.ChangeHeadNurseStatus
{
    public class ChangeHeadNurseStatusHandler :
        IRequestHandler<ChangeHeadNurseStatusCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public ChangeHeadNurseStatusHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(ChangeHeadNurseStatusCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId
                ?? throw new UnauthorizedAccessException("User must be logged in.");

            var isAdmin = _currentUserService.IsInRole("Admin");

            if (!isAdmin)
            {
                throw new ForbiddenAccessException();
            }

            var nurseToUpdate = await _unitOfWork.Nurse.GetByIdAsync(request.nurseId)
                ?? throw new NotFoundException(nameof(Nurse), request.nurseId);
            nurseToUpdate.ChangeHeadNurseStatus(request.isHead);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }

    }
}
