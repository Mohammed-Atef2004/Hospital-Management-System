using Application.Common.Exceptions;
using Domain.Interfaces.Services;
using Domain.PatientAssignments;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Nurses.Commands.TransferPatientToAnotherNurse
{
    internal class TransferPatientToAnotherNurseHandler : IRequestHandler<TransferPatientToAnotherNurseCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public TransferPatientToAnotherNurseHandler(
            IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(TransferPatientToAnotherNurseCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId
                ?? throw new UnauthorizedAccessException("User must be logged in.");

            var currentUserStaff = await _unitOfWork.Nurse.GetByIdAsync(currentUserId)
                ?? throw new NotFoundException("Nurse", currentUserId);

            if (!currentUserStaff.IsHeadNurse)
                throw new ForbiddenAccessException();

            var oldAssignment = await _unitOfWork.PatientAssignment.GetByIdAsync(request.AssignmentId)
                ?? throw new NotFoundException(nameof(PatientAssignment), request.AssignmentId);


            var targetNurse = await _unitOfWork.Nurse.GetByIdWithShiftsAndAssignmentsAsync(request.nurseId, cancellationToken)
                ?? throw new NotFoundException("Target Nurse", request.nurseId);
            oldAssignment.EndAssignment();


            targetNurse.AssignPatient(
                oldAssignment.PatientId,
                request.shiftId,
                currentUserId
            );

            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
