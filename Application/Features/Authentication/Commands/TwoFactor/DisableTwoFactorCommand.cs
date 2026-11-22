using Application.Common;

namespace Application.Features.Authentication.Commands.TwoFactor;

public sealed record DisableTwoFactorCommand(Guid UserId, string CurrentPassword, string Code)
    : ICommandRequest<MediatR.Unit>;