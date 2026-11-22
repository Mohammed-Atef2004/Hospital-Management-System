using FluentValidation;

namespace Application.Features.Authentication.Commands.TwoFactor;

public sealed class VerifyTwoFactorSetupCommandValidator : AbstractValidator<VerifyTwoFactorSetupCommand>
{
    public VerifyTwoFactorSetupCommandValidator()
    {
        RuleFor(command => command.UserId).NotEmpty();
        RuleFor(command => command.Code).Matches("^[0-9]{6}$");
    }
}

public sealed class StartTwoFactorSetupCommandValidator : AbstractValidator<StartTwoFactorSetupCommand>
{
    public StartTwoFactorSetupCommandValidator() => RuleFor(command => command.UserId).NotEmpty();
}

public sealed class DisableTwoFactorCommandValidator : AbstractValidator<DisableTwoFactorCommand>
{
    public DisableTwoFactorCommandValidator()
    {
        RuleFor(command => command.UserId).NotEmpty();
        RuleFor(command => command.CurrentPassword).NotEmpty().MaximumLength(128);
        RuleFor(command => command.Code).Matches("^[0-9]{6}$");
    }
}