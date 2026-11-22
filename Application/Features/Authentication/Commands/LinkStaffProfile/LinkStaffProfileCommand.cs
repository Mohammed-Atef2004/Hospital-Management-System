using Application.Common;
using MediatR;

namespace Application.Features.Authentication.Commands.LinkStaffProfile;

public sealed record LinkStaffProfileCommand(Guid UserId, int StaffId) : ICommandRequest<Unit>;
