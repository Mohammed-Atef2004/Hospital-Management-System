using Application.Common;
using Application.Features.Patients.DTOs;

namespace Application.Features.Patients.Commands.UpsertMyProfile;

public sealed record UpsertMyPatientProfileCommand(
    Guid UserId, string FirstName, string LastName, string Address, string? City, string? PhoneNumber)
    : ICommandRequest<PatientProfileDto>;