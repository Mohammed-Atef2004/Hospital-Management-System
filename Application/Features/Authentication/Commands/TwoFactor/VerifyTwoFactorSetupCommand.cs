using Application.Common;

namespace Application.Features.Authentication.Commands.TwoFactor;

public sealed record VerifyTwoFactorSetupCommand(Guid UserId, string Code) : ICommandRequest<MediatR.Unit>;