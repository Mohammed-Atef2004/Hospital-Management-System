using Application.Features.Authentication;
using Domain.SharedKernel;
using Domain.Users;
using Infastructure.Presistence.Database;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Authentication.Commands.Login;

public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthResponse>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _users;
    private readonly ApplicationDbContext _dbContext;
    private readonly IAuthTokenService _tokens;
    private readonly IMediator _mediator;

    public LoginUserCommandHandler(
        UserManager<IdentityUser> userManager,
        IUserRepository users,
        ApplicationDbContext dbContext,
        IAuthTokenService tokens,
        IMediator mediator)
    {
        _userManager = userManager;
        _users = users;
        _dbContext = dbContext;
        _tokens = tokens;
        _mediator = mediator;
    }

    public async Task<Result<AuthResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var identityUser = request.UsernameOrEmail.Contains('@')
            ? await _userManager.FindByEmailAsync(request.UsernameOrEmail.Trim())
            : await _userManager.FindByNameAsync(request.UsernameOrEmail.Trim());
        if (identityUser is null)
            return Result<AuthResponse>.Failure(AuthenticationErrors.InvalidCredentials);

        var user = await _users.GetByIdentityIdAsync(identityUser.Id, cancellationToken);
        if (user is null || user.IsDeleted || !user.IsActive)
            return Result<AuthResponse>.Failure(AuthenticationErrors.InvalidCredentials);

        if (!await _userManager.CheckPasswordAsync(identityUser, request.Password))
        {
            var failedLogin = user.RecordFailedLogin(request.IpAddress);
            if (failedLogin.IsSuccess)
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                await PublishEventsAsync(user, cancellationToken);
            }
            return Result<AuthResponse>.Failure(AuthenticationErrors.InvalidCredentials);
        }

        var loginResult = user.RecordSuccessfulLogin(request.IpAddress);
        if (loginResult.IsFailure)
            return Result<AuthResponse>.Failure(AuthenticationErrors.InvalidCredentials);

        await _dbContext.SaveChangesAsync(cancellationToken);
        await PublishEventsAsync(user, cancellationToken);

        var (token, expiresAtUtc) = _tokens.CreateToken(user);
        return Result<AuthResponse>.Success(new AuthResponse(
            token, expiresAtUtc, user.Id, user.Username.Value, user.Role.ToString()));
    }

    private async Task PublishEventsAsync(User user, CancellationToken cancellationToken)
    {
        foreach (var domainEvent in user.DomainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);
        user.ClearDomainEvents();
    }
}
