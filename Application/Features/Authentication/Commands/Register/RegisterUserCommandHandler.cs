using Application.Features.Authentication;
using Domain.SharedKernel;
using Domain.Repositories;
using Domain.Users;
using Domain.Users.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Authentication.Commands.Register;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<AuthResponse>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _users;
    private readonly Application.Common.ITransactionManager _transactions;
    private readonly IAuthTokenService _tokens;
    private readonly IRefreshTokenService _refreshTokens;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        UserManager<IdentityUser> userManager,
        IUserRepository users,
        Application.Common.ITransactionManager transactions,
        IAuthTokenService tokens,
        IRefreshTokenService refreshTokens,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _users = users;
        _transactions = transactions;
        _tokens = tokens;
        _refreshTokens = refreshTokens;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure) return Result<AuthResponse>.Failure(emailResult.Error);
        var usernameResult = Username.Create(request.Username);
        if (usernameResult.IsFailure) return Result<AuthResponse>.Failure(usernameResult.Error);

        var domainResult = User.Create(
            identityId: Guid.NewGuid().ToString("N"),
            email: emailResult.Value.Value,
            username: usernameResult.Value.Value,
            firstName: request.FirstName,
            lastName: request.LastName,
            isEmailTaken: await _users.ExistsByEmailAsync(emailResult.Value, cancellationToken),
            isUsernameTaken: await _users.ExistsByUsernameAsync(usernameResult.Value, cancellationToken));

        if (domainResult.IsFailure)
            return Result<AuthResponse>.Failure(domainResult.Error);

        var user = domainResult.Value;
        var identityUser = new IdentityUser
        {
            Id = user.IdentityId,
            UserName = user.Username.Value,
            Email = user.Email.Value
        };
        var identityResult = await _userManager.CreateAsync(identityUser, request.Password);
        if (!identityResult.Succeeded)
        {
            var descriptions = string.Join(" ", identityResult.Errors.Select(error => error.Description));
            return Result<AuthResponse>.Failure(new Error(
                AuthenticationErrors.RegistrationFailed.Code,
                string.IsNullOrWhiteSpace(descriptions) ? AuthenticationErrors.RegistrationFailed.Message : descriptions));
        }

        var initialPassword = user.RecordPasswordChange(identityUser.PasswordHash!, isPasswordReused: false);
        if (initialPassword.IsFailure)
        {
            await _userManager.DeleteAsync(identityUser);
            return Result<AuthResponse>.Failure(initialPassword.Error);
        }

        var loginResult = user.RecordSuccessfulLogin("registration");
        if (loginResult.IsFailure)
        {
            await _userManager.DeleteAsync(identityUser);
            return Result<AuthResponse>.Failure(loginResult.Error);
        }

        await _users.AddAsync(user);
        var refreshToken = _refreshTokens.Create();
        await _unitOfWork.RefreshTokens.AddAsync(
            Domain.Users.RefreshToken.Issue(user.Id, refreshToken.TokenHash, refreshToken.ExpiresAtUtc));
        await _transactions.SaveChangesAsync(cancellationToken);

        var (token, expiresAtUtc) = _tokens.CreateToken(user);
        return Result<AuthResponse>.Success(new AuthResponse(
            token, expiresAtUtc, user.Id, user.Username.Value, user.Role.ToString(),
            refreshToken.Token, refreshToken.ExpiresAtUtc));
    }
}
