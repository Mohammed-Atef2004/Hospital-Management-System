using Domain.SharedKernel;
using MediatR;

namespace Application.Features.Authentication.Commands.ChangePassword;

public sealed record ChangePasswordCommand(string IdentityId, string CurrentPassword, string NewPassword)
    : IRequest<Result>;
