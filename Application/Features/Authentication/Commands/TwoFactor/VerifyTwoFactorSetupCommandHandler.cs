using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Authentication.Commands.TwoFactor;

public sealed class VerifyTwoFactorSetupCommandHandler : IRequestHandler<VerifyTwoFactorSetupCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITwoFactorCodeService _codes;

    public VerifyTwoFactorSetupCommandHandler(IUnitOfWork unitOfWork, ITwoFactorCodeService codes)
    {
        _unitOfWork = unitOfWork;
        _codes = codes;
    }

    public async Task<Unit> Handle(VerifyTwoFactorSetupCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.User.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException("User", request.UserId);
        if (string.IsNullOrWhiteSpace(user.PendingTwoFactorSecret) ||
            !_codes.VerifyCode(user.PendingTwoFactorSecret, request.Code, DateTimeOffset.UtcNow))
            throw new BadRequestException("The authenticator code is invalid or setup has not been started.");

        var result = user.CompleteTwoFactorSetup();
        if (result.IsFailure) throw new BadRequestException(result.Error.Message);

        await _unitOfWork.CompleteAsync();
        return Unit.Value;
    }
}