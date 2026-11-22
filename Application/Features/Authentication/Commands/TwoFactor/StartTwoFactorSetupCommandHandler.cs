using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Authentication.Commands.TwoFactor;

public sealed class StartTwoFactorSetupCommandHandler : IRequestHandler<StartTwoFactorSetupCommand, TwoFactorSetupResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITwoFactorCodeService _codes;

    public StartTwoFactorSetupCommandHandler(IUnitOfWork unitOfWork, ITwoFactorCodeService codes)
    {
        _unitOfWork = unitOfWork;
        _codes = codes;
    }

    public async Task<TwoFactorSetupResponse> Handle(StartTwoFactorSetupCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.User.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException("User", request.UserId);
        var secret = _codes.GenerateSecret();
        var result = user.BeginTwoFactorSetup(secret);
        if (result.IsFailure) throw new BadRequestException(result.Error.Message);

        await _unitOfWork.CompleteAsync();
        var uri = _codes.BuildProvisioningUri(secret, user.Email.Value, "Hospital Management System");
        return new TwoFactorSetupResponse(secret, uri);
    }
}