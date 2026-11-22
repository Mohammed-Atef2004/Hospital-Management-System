using Application.Common;
using Domain.SharedKernel;
using MediatR;

namespace Application.Features.Authentication.Commands.Login;

public sealed record LoginUserCommand(string UsernameOrEmail, string Password, string IpAddress)
    : ICommandRequest<Result<AuthResponse>>, ICommitTransactionOnFailure;
