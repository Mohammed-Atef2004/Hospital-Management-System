using Application.Common;

namespace Application.Features.Authentication.Commands.TwoFactor;

public sealed record StartTwoFactorSetupCommand(Guid UserId) : ICommandRequest<TwoFactorSetupResponse>;
public sealed record TwoFactorSetupResponse(string Secret, string ProvisioningUri);