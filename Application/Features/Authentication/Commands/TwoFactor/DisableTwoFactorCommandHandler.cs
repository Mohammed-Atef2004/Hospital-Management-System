using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Authentication.Commands.TwoFactor;

public sealed class DisableTwoFactorCommandHandler : IRequestHandler<DisableTwoFactorCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITwoFactorCodeService _codes;

    public DisableTwoFactorCommandHandler(
        IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager,
        ITwoFactorCodeService codes)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _codes = codes;
    }

    public async Task<Unit> Handle(DisableTwoFactorCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.User.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException("User", request.UserId);
        var identityUser = await _userManager.FindByIdAsync(user.IdentityId);
        if (identityUser is null || !await _userManager.CheckPasswordAsync(identityUser, request.CurrentPassword))
            throw new BadRequestException("The supplied credentials are invalid.");
        if (!user.IsTwoFactorEnabled || string.IsNullOrWhiteSpace(user.TwoFactorSecret) ||
            !_codes.VerifyCode(user.TwoFactorSecret, request.Code, DateTimeOffset.UtcNow))
            throw new BadRequestException("The authenticator code is invalid or two-factor authentication is not enabled.");

        var result = user.DisableTwoFactor();
        if (result.IsFailure) throw new BadRequestException(result.Error.Message);

        await _unitOfWork.CompleteAsync();
        return Unit.Value;
    }
}