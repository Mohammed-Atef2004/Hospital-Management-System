using Domain.Users;

namespace Application.Features.Authentication;

public interface IAuthTokenService
{
    (string Token, DateTime ExpiresAtUtc) CreateToken(User user);
}
