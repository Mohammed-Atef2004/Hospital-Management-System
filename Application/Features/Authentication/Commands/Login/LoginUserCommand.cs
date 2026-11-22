using Application.Common;
using Domain.SharedKernel;
using MediatR;

namespace Application.Features.Authentication.Commands.Login;

public sealed record LoginUserCommand(string UsernameOrEmail, string Password, string IpAddress, string? TwoFactorCode = null)
    : ICommandRequest<Result<AuthResponse>>, ICommitTransactionOnFailure;
