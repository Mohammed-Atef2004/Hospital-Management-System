using Application.Common;
using Domain.SharedKernel;

namespace Application.Features.Authentication.Commands.RefreshToken;

public sealed record RefreshAccessTokenCommand(string RefreshToken) : ICommandRequest<Result<AuthResponse>>;