using Application.Features.Authentication;
using Domain.SharedKernel;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Authentication.Commands.Login;

public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthResponse>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _users;
    private readonly Application.Common.ITransactionManager _transactions;
    private readonly IAuthTokenService _tokens;

    public LoginUserCommandHandler(
        UserManager<IdentityUser> userManager,
        IUserRepository users,
        Application.Common.ITransactionManager transactions,
        IAuthTokenService tokens)
    {
        _userManager = userManager;
        _users = users;
        _transactions = transactions;
        _tokens = tokens;
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
                await _transactions.SaveChangesAsync(cancellationToken);
            }
            return Result<AuthResponse>.Failure(AuthenticationErrors.InvalidCredentials);
        }

        var loginResult = user.RecordSuccessfulLogin(request.IpAddress);
        if (loginResult.IsFailure)
            return Result<AuthResponse>.Failure(AuthenticationErrors.InvalidCredentials);

        await _transactions.SaveChangesAsync(cancellationToken);
        var (token, expiresAtUtc) = _tokens.CreateToken(user);
        return Result<AuthResponse>.Success(new AuthResponse(
            token, expiresAtUtc, user.Id, user.Username.Value, user.Role.ToString()));
    }

}
