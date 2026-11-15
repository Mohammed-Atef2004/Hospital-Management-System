using Application.Common;
using Domain.SharedKernel;
using MediatR;

namespace Application.Features.Authentication.Commands.Register;

public sealed record RegisterUserCommand(
    string Email,
    string Username,
    string FirstName,
    string LastName,
    string Password) : ICommandRequest<Result<AuthResponse>>;
