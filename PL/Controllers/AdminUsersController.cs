using Application.Features.Authentication.Commands.LinkStaffProfile;
using Application.Features.Authentication.Commands.ChangeUserRole;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Policy = "Administrative")]
public sealed class AdminUsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminUsersController(IMediator mediator) => _mediator = mediator;

    [HttpPut("{userId:guid}/staff/{staffId:int}")]
    public async Task<IActionResult> LinkStaffProfile(Guid userId, int staffId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new LinkStaffProfileCommand(userId, staffId), cancellationToken);
        return NoContent();
    }

    [HttpPatch("{userId:guid}/role")]
    public async Task<IActionResult> ChangeRole(Guid userId, ChangeUserRoleRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ChangeUserRoleCommand(userId, request.Role), cancellationToken);
        return NoContent();
    }
}

public sealed record ChangeUserRoleRequest(UserRole Role);
