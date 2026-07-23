using Application.Common.Exceptions;
using Domain.Interfaces.Services;
using Domain.PatientAssignments;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Nurses.Commands.EndPatientAssignment
{
    public class EndPatientAssignmentHandler : IRequestHandler<EndPatientAssignmentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public EndPatientAssignmentHandler(
            IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(EndPatientAssignmentCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId
                ?? throw new UnauthorizedAccessException("User must be logged in.");

            var currentUser = await _unitOfWork.Nurse.GetByIdAsync(currentUserId)
                ?? throw new UnauthorizedAccessException("Invalid user.");

            if (!currentUser.IsHeadNurse)
                throw new ForbiddenAccessException();

            var assignment = await _unitOfWork.PatientAssignment.GetByIdAsync(request.AssignmentId)
                ?? throw new NotFoundException(nameof(PatientAssignment), request.AssignmentId);

            assignment.EndAssignment();
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
