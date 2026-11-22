using Application.Features.Authentication;
using Domain.SharedKernel;
using Domain.Users;
using Domain.Users.Errors;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Authentication.Commands.ChangePassword;

public sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _users;
    private readonly Application.Common.ITransactionManager _transactions;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordCommandHandler(
        UserManager<IdentityUser> userManager,
        IUserRepository users,
        Application.Common.ITransactionManager transactions,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _users = users;
        _transactions = transactions;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var identityUser = await _userManager.FindByIdAsync(request.IdentityId);
        if (identityUser is null || !await _userManager.CheckPasswordAsync(identityUser, request.CurrentPassword))
            return Result.Failure(AuthenticationErrors.InvalidCredentials);

        var user = await _users.GetByIdentityIdAsync(identityUser.Id, cancellationToken);
        if (user is null || user.IsDeleted || !user.IsActive)
            return Result.Failure(AuthenticationErrors.InvalidCredentials);

        var passwordHasher = _userManager.PasswordHasher;
        var passwordWasUsed = user.PasswordHashes.Any(hash =>
            passwordHasher.VerifyHashedPassword(identityUser, hash, request.NewPassword)
            != PasswordVerificationResult.Failed);
        if (passwordWasUsed)
            return Result.Failure(UserErrors.SecurityErrors.PasswordAlreadyUsed);

        var identityResult = await _userManager.ChangePasswordAsync(
            identityUser, request.CurrentPassword, request.NewPassword);
        if (!identityResult.Succeeded)
        {
            var descriptions = string.Join(" ", identityResult.Errors.Select(error => error.Description));
            return Result.Failure(new Error(
                AuthenticationErrors.RegistrationFailed.Code,
                string.IsNullOrWhiteSpace(descriptions) ? AuthenticationErrors.RegistrationFailed.Message : descriptions));
        }

        var domainResult = user.RecordPasswordChange(identityUser.PasswordHash!, isPasswordReused: false);
        if (domainResult.IsFailure) return domainResult;

        await _unitOfWork.RefreshTokens.RevokeAllForUserAsync(user.Id, DateTime.UtcNow, cancellationToken);

        await _transactions.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
