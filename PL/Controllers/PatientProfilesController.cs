using Application.Features.Patients.Commands.UpsertMyProfile;
using Application.Features.Patients.Queries.GetMyProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/patients/me")]
[Authorize(Policy = "Patients")]
public sealed class PatientProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientProfilesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        return Ok(await _mediator.Send(new GetMyPatientProfileQuery(userId), cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> UpsertMyProfile(
        UpsertMyPatientProfileRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        var profile = await _mediator.Send(new UpsertMyPatientProfileCommand(
            userId, request.FirstName, request.LastName, request.Address, request.City, request.PhoneNumber),
            cancellationToken);
        return Ok(profile);
    }

    private bool TryGetUserId(out Guid userId)
        => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
}

public sealed record UpsertMyPatientProfileRequest(
    string FirstName, string LastName, string Address, string? City, string? PhoneNumber);
