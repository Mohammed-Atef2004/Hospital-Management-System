using MediatR;

namespace Application.Features.Nurses.Commands.TransferPatientToAnotherNurse
{
    public record TransferPatientToAnotherNurseCommand(
            int AssignmentId,
            int nurseId,
            int shiftId
       ) : IRequest<Unit>;
}
