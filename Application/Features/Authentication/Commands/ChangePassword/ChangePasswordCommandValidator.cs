using FluentValidation;

namespace Application.Features.Authentication.Commands.ChangePassword;

public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(command => command.IdentityId).NotEmpty().MaximumLength(450);
        RuleFor(command => command.CurrentPassword).NotEmpty().MaximumLength(128);
        RuleFor(command => command.NewPassword).NotEmpty().MinimumLength(8).MaximumLength(128)
            .Matches("[A-Z]").Matches("[a-z]").Matches("[0-9]").Matches("[^a-zA-Z0-9]");
    }
}
