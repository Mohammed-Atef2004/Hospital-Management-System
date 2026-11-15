using Application.Common;
using MediatR;

namespace Application.Features.Nurses.Commands.TransferPatientToAnotherNurse
{
    public record TransferPatientToAnotherNurseCommand(
            int AssignmentId,
            int nurseId,
            int shiftId
       ) : ICommandRequest<Unit>;
}
