using Application.Common.Exceptions;
using Application.Features.Patients.DTOs;
using Domain.Repositories;
using Domain.Users;
using MediatR;

namespace Application.Features.Patients.Queries.GetMyProfile;

public sealed class GetMyPatientProfileQueryHandler : IRequestHandler<GetMyPatientProfileQuery, PatientProfileDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMyPatientProfileQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<PatientProfileDto> Handle(GetMyPatientProfileQuery request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.User.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException("User", request.UserId);
        if (!account.IsActive || account.IsDeleted || account.Role != UserRole.Patient)
            throw new ForbiddenAccessException();

        var patient = await _unitOfWork.Patient.GetByUserIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException("Patient profile", request.UserId);
        return new PatientProfileDto(patient.Id, patient.FirstName, patient.LastName,
            patient.Address, patient.City, patient.PhoneNumber);
    }
}