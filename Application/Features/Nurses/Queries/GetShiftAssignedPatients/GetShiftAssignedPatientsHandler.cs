using Application.Common.Exceptions;
using Application.Features.Nurses.DTOs;
using AutoMapper;
using Domain.Interfaces.Services;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Nurses.Queries.GetShiftAssignedPatients
{
    public class GetShiftAssignedPatientsHandler : IRequestHandler<GetShiftAssignedPatientsQuery, List<PatientDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public GetShiftAssignedPatientsHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<List<PatientDTO>> Handle(GetShiftAssignedPatientsQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId
                ?? throw new UnauthorizedAccessException("User must be logged in.");

            if (currentUserId != request.NurseId)
            {
                var currentUser = await _unitOfWork.Nurse.GetByIdAsync(currentUserId);

                var isSupervisor = _currentUserService.IsInRole("Admin") || (currentUser != null && currentUser.IsHeadNurse);

                if (!isSupervisor)
                {
                    throw new ForbiddenAccessException();
                }
            }
            var shift = await _unitOfWork.Shift
            .GetByIdAsync(request.ShitfId);

            if (shift == null)
                throw new NotFoundException("Shift not found.");

            if (shift.StaffId != request.NurseId)
                throw new BadRequestException(
                    "This shift does not belong to this nurse.");
            var patients = await _unitOfWork.PatientAssignment
                .GetPatientsByShiftAndNurseAsync(request.ShitfId, request.NurseId, cancellationToken);
            return _mapper.Map<List<PatientDTO>>(patients);
        }
    }
}
