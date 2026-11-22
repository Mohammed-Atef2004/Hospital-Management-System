using Domain.SharedKernel;

namespace Application.Features.Authentication;

public static class AuthenticationErrors
{
    public static readonly Error InvalidCredentials =
        new("Auth.InvalidCredentials", "The username/email or password is incorrect.");

    public static readonly Error TwoFactorVerificationRequired =
        new("Auth.TwoFactorVerificationRequired", "Two-factor verification is required for this account.");

    public static readonly Error InvalidRefreshToken =
        new("Auth.InvalidRefreshToken", "The refresh token is invalid, expired, revoked, or no longer valid.");

    public static readonly Error RegistrationFailed =
        new("Auth.RegistrationFailed", "The account could not be registered.");

    public static readonly Error PersistenceFailed =
        new("Auth.PersistenceFailed", "The account could not be saved.");
}
