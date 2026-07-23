using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Nurses;
using Domain.Repositories;
using Domain.SharedKernel;
using MediatR;

namespace Application.Features.Nurses.Commands.RecordVitalSigns
{
    public class RecordVitalSignsHandler : IRequestHandler<RecordVitalSignsCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public RecordVitalSignsHandler(
            IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(RecordVitalSignsCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId
                ?? throw new UnauthorizedAccessException("User must be logged in.");

            var nurse = await _unitOfWork.Nurse.GetByIdWithShiftsAndAssignmentsAsync(request.NurseId, cancellationToken)
                ?? throw new NotFoundException(nameof(Nurse), request.NurseId);

            var patient = await _unitOfWork.Patient.GetByIdAsync(request.PatientId)
                ?? throw new NotFoundException(nameof(Patient), request.PatientId);

            var isAssigned = nurse.PatientAssignments.Any(a => a.PatientId == request.PatientId && a.IsActive);
            if (!isAssigned)
            {
                throw new DomainException("This nurse is not currently assigned to this patient.");
            }

            var newVitalSign = patient.RecordVitalSigns(
                request.NurseId,
                request.Temperature,
                request.HeartRate,
                request.SystolicBloodPressure,
                request.DiastolicBloodPressure,
                request.RespiratoryRate,
                request.OxygenSaturation);

            await _unitOfWork.CompleteAsync();
            return newVitalSign.Id;
        }
    }
}

