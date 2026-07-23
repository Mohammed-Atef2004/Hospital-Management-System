using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Nurses;
using Domain.Repositories;
using Domain.Shifts;
using MediatR;

namespace Application.Features.Nurses.Commands.AssignPatientToNurse
{
    public class AssignPatientToNurseCommandHandler : IRequestHandler<AssignPatientToNurseCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AssignPatientToNurseCommandHandler(
            IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(AssignPatientToNurseCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId
                ?? throw new UnauthorizedAccessException("User must be logged in.");
            var shiftExists = await _unitOfWork.Shift.ExistsAsync(s => s.Id == request.shiftId, cancellationToken);
            if (!shiftExists)
            {
                throw new NotFoundException(nameof(Shift), request.shiftId);
            }

            var patientExists = await _unitOfWork.Patient.ExistsAsync(s => s.Id == request.patientId, cancellationToken);
            if (!patientExists)
            {
                throw new NotFoundException(nameof(Patient), request.patientId);
            }

            var currentUserNurse = await _unitOfWork.Nurse.GetByIdAsync(currentUserId);
            if (currentUserNurse == null || !currentUserNurse.IsHeadNurse)
            {
                throw new ForbiddenAccessException();
            }
            var targetNurse = await _unitOfWork.Nurse.GetByIdWithShiftsAndAssignmentsAsync(request.nurseId, cancellationToken);
            if (targetNurse == null)
            {
                throw new NotFoundException(nameof(Nurse), request.nurseId);
            }

            var isNurseAssignedToShift = targetNurse.Shifts.Any(s => s.Id == request.shiftId);
            if (!isNurseAssignedToShift)
            {
                throw new KeyNotFoundException($"Nurse with ID {request.nurseId} is not assigned to work in Shift {request.shiftId}.");
            }
            var newAssignment = targetNurse.AssignPatient(request.patientId, request.shiftId, currentUserId);

            await _unitOfWork.CompleteAsync();

            return newAssignment.Id;
        }
    }
}

