using Application.Common;
using Application.Features.Patients.DTOs;
using MediatR;

namespace Application.Features.Patients.Queries.GetMyProfile;

public sealed record GetMyPatientProfileQuery(Guid UserId) : IRequest<PatientProfileDto>;
