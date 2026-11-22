using FluentValidation;

namespace Application.Features.Authentication.Commands.RefreshToken;

public sealed class RefreshAccessTokenCommandValidator : AbstractValidator<RefreshAccessTokenCommand>
{
    public RefreshAccessTokenCommandValidator() => RuleFor(command => command.RefreshToken).NotEmpty().MaximumLength(256);
}