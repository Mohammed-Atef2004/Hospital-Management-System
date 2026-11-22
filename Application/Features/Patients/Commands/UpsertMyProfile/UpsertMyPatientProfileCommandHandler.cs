using Application.Common.Exceptions;
using Application.Features.Patients.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Domain.Users;
using MediatR;

namespace Application.Features.Patients.Commands.UpsertMyProfile;

public sealed class UpsertMyPatientProfileCommandHandler
    : IRequestHandler<UpsertMyPatientProfileCommand, PatientProfileDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpsertMyPatientProfileCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<PatientProfileDto> Handle(
        UpsertMyPatientProfileCommand request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.User.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException("User", request.UserId);
        if (!account.IsActive || account.IsDeleted)
            throw new ForbiddenAccessException();
        if (account.Role != UserRole.Patient)
            throw new ForbiddenAccessException();

        var patient = await _unitOfWork.Patient.GetByUserIdAsync(request.UserId, cancellationToken);
        if (patient is null)
        {
            patient = Patient.RegisterProfile(request.UserId, request.FirstName, request.LastName,
                request.Address, request.City, request.PhoneNumber);
            await _unitOfWork.Patient.AddAsync(patient);
        }
        else
        {
            patient.UpdateProfile(request.FirstName, request.LastName,
                request.Address, request.City, request.PhoneNumber);
        }

        await _unitOfWork.CompleteAsync();
        return new PatientProfileDto(patient.Id, patient.FirstName, patient.LastName,
            patient.Address, patient.City, patient.PhoneNumber);
    }
}