using Application.Common;
using Domain.Users;
using MediatR;

namespace Application.Features.Authentication.Commands.ChangeUserRole;

public sealed record ChangeUserRoleCommand(Guid UserId, UserRole Role) : ICommandRequest<Unit>;
