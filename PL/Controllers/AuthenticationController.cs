using Application.Features.Authentication.Commands.Login;
using Application.Features.Authentication.Commands.Register;
using Application.Features.Authentication.Commands.ChangePassword;
using Application.Features.Authentication.Commands.TwoFactor;
using Application.Features.Authentication.Commands.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

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
            new LoginUserCommand(request.UsernameOrEmail, request.Password, ipAddress, request.TwoFactorCode), cancellationToken);
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

    [HttpPost("two-factor/setup")]
    public async Task<IActionResult> StartTwoFactorSetup(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        return Ok(await _mediator.Send(new StartTwoFactorSetupCommand(userId), cancellationToken));
    }

    [HttpPost("two-factor/verify-setup")]
    public async Task<IActionResult> VerifyTwoFactorSetup(
        VerifyTwoFactorSetupRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        await _mediator.Send(new VerifyTwoFactorSetupCommand(userId, request.Code), cancellationToken);
        return NoContent();
    }

    [HttpDelete("two-factor")]
    public async Task<IActionResult> DisableTwoFactor(
        DisableTwoFactorRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        await _mediator.Send(new DisableTwoFactorCommand(userId, request.CurrentPassword, request.Code), cancellationToken);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RefreshAccessTokenCommand(request.RefreshToken), cancellationToken);
        if (result.IsFailure) return Unauthorized(new { result.Error.Code, result.Error.Message });
        return Ok(result.Value);
    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshToken(
        RevokeRefreshTokenRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();
        await _mediator.Send(new RevokeRefreshTokenCommand(userId, request.RefreshToken), cancellationToken);
        return NoContent();
    }

    private bool TryGetUserId(out Guid userId)
        => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
}

public sealed record RegisterUserRequest(string Email, string Username, string FirstName, string LastName, string Password);
public sealed record LoginUserRequest(string UsernameOrEmail, string Password, string? TwoFactorCode = null);
public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);
public sealed record VerifyTwoFactorSetupRequest(string Code);
public sealed record DisableTwoFactorRequest(string CurrentPassword, string Code);
public sealed record RefreshTokenRequest(string RefreshToken);
public sealed record RevokeRefreshTokenRequest(string RefreshToken);
