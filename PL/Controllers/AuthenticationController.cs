using Application.Features.Authentication.Commands.Login;
using Application.Features.Authentication.Commands.Register;
using Application.Features.Authentication.Commands.ChangePassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
[EnableRateLimiting("auth")]
public sealed class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator) => _mediator = mediator;

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RegisterUserCommand(
            request.Email, request.Username, request.FirstName, request.LastName, request.Password), cancellationToken);
        if (result.IsSuccess) return Ok(result.Value);

        var status = result.Error.Code is "User.Rules.EmailMustBeUnique" or "User.Rules.UsernameMustBeUnique"
            ? StatusCodes.Status409Conflict
            : StatusCodes.Status400BadRequest;
        return StatusCode(status, new { result.Error.Code, result.Error.Message });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var result = await _mediator.Send(
            new LoginUserCommand(request.UsernameOrEmail, request.Password, ipAddress), cancellationToken);
        if (result.IsFailure)
            return Unauthorized(new { result.Error.Code, result.Error.Message });
        return Ok(result.Value);
    }

    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var identityId = User.FindFirst("identity_id")?.Value;
        if (string.IsNullOrWhiteSpace(identityId)) return Unauthorized();

        var result = await _mediator.Send(
            new ChangePasswordCommand(identityId, request.CurrentPassword, request.NewPassword), cancellationToken);
        if (result.IsFailure)
        {
            if (result.Error == Application.Features.Authentication.AuthenticationErrors.InvalidCredentials)
                return Unauthorized(new { result.Error.Code, result.Error.Message });
            return BadRequest(new { result.Error.Code, result.Error.Message });
        }
        return NoContent();
    }
}

public sealed record RegisterUserRequest(string Email, string Username, string FirstName, string LastName, string Password);
public sealed record LoginUserRequest(string UsernameOrEmail, string Password);
public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);
