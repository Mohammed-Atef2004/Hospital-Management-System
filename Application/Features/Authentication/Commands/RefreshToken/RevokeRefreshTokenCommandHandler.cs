using Domain.Repositories;
using MediatR;

namespace Application.Features.Authentication.Commands.RefreshToken;

public sealed class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenService _refreshTokens;

    public RevokeRefreshTokenCommandHandler(IUnitOfWork unitOfWork, IRefreshTokenService refreshTokens)
    {
        _unitOfWork = unitOfWork;
        _refreshTokens = refreshTokens;
    }

    public async Task<Unit> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var hash = _refreshTokens.Hash(request.RefreshToken);
        var token = await _unitOfWork.RefreshTokens.GetByTokenHashAsync(hash, cancellationToken);
        if (token is not null && token.UserId == request.UserId)
        {
            token.Revoke(DateTime.UtcNow);
            await _unitOfWork.CompleteAsync();
        }
        return Unit.Value;
    }
}