using Domain.SharedKernel;

namespace Application.Features.Authentication;

public static class AuthenticationErrors
{
    public static readonly Error InvalidCredentials =
        new("Auth.InvalidCredentials", "The username/email or password is incorrect.");

    public static readonly Error RegistrationFailed =
        new("Auth.RegistrationFailed", "The account could not be registered.");

    public static readonly Error PersistenceFailed =
        new("Auth.PersistenceFailed", "The account could not be saved.");
}
