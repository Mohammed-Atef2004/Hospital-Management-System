using MediatR;

namespace Application.Features.Nurses.Commands.AssignPatientToNurse
{
    public record AssignPatientToNurseCommand(
            int patientId,
            int nurseId,
            int shiftId
       ) : IRequest<int>;

}
