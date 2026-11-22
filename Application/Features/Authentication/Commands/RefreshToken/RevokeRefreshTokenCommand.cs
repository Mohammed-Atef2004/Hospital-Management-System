using Application.Common;
using MediatR;

namespace Application.Features.Authentication.Commands.RefreshToken;

public sealed record RevokeRefreshTokenCommand(Guid UserId, string RefreshToken) : ICommandRequest<Unit>;