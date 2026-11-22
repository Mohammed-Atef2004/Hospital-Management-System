using Application.Features.Authentication;
using Domain.Repositories;
using Domain.SharedKernel;
using MediatR;

namespace Application.Features.Authentication.Commands.RefreshToken;

public sealed class RefreshAccessTokenCommandHandler
    : IRequestHandler<RefreshAccessTokenCommand, Result<AuthResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthTokenService _accessTokens;
    private readonly IRefreshTokenService _refreshTokens;

    public RefreshAccessTokenCommandHandler(
        IUnitOfWork unitOfWork, IAuthTokenService accessTokens, IRefreshTokenService refreshTokens)
    {
        _unitOfWork = unitOfWork;
        _accessTokens = accessTokens;
        _refreshTokens = refreshTokens;
    }

    public async Task<Result<AuthResponse>> Handle(
        RefreshAccessTokenCommand request, CancellationToken cancellationToken)
    {
        string tokenHash;
        try { tokenHash = _refreshTokens.Hash(request.RefreshToken); }
        catch (ArgumentException) { return Result<AuthResponse>.Failure(AuthenticationErrors.InvalidRefreshToken); }

        var storedToken = await _unitOfWork.RefreshTokens.GetByTokenHashAsync(tokenHash, cancellationToken);
        var now = DateTime.UtcNow;
        if (storedToken is null || !storedToken.IsUsable(now))
            return Result<AuthResponse>.Failure(AuthenticationErrors.InvalidRefreshToken);

        var user = await _unitOfWork.User.GetByIdAsync(storedToken.UserId);
        if (user is null || !user.IsActive || user.IsDeleted || user.IsAuthenticationLockedOut(now))
            return Result<AuthResponse>.Failure(AuthenticationErrors.InvalidRefreshToken);

        var replacement = _refreshTokens.Create();
        storedToken.Revoke(now, replacement.TokenHash);
        await _unitOfWork.RefreshTokens.AddAsync(
            Domain.Users.RefreshToken.Issue(user.Id, replacement.TokenHash, replacement.ExpiresAtUtc));
        await _unitOfWork.CompleteAsync();

        var (accessToken, accessExpiry) = _accessTokens.CreateToken(user);
        return Result<AuthResponse>.Success(new AuthResponse(
            accessToken, accessExpiry, user.Id, user.Username.Value, user.Role.ToString(),
            replacement.Token, replacement.ExpiresAtUtc));
    }
}
