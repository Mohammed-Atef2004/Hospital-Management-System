using FluentValidation;

namespace Application.Features.Authentication.Commands.RefreshToken;

public sealed class RevokeRefreshTokenCommandValidator : AbstractValidator<RevokeRefreshTokenCommand>
{
    public RevokeRefreshTokenCommandValidator()
    {
        RuleFor(command => command.UserId).NotEmpty();
        RuleFor(command => command.RefreshToken).NotEmpty().MaximumLength(256);
    }
}
