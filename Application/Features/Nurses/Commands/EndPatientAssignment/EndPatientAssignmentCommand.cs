using MediatR;

namespace Application.Features.Nurses.Commands.EndPatientAssignment
{
    public record EndPatientAssignmentCommand(
            int AssignmentId
       ) : IRequest<Unit>;
}

