using Application.Common;
using MediatR;

namespace Application.Features.Nurses.Commands.ChangeHeadNurseStatus
{
    public record ChangeHeadNurseStatusCommand
    (int nurseId, bool isHead) : ICommandRequest<Unit>;
}
