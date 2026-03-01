using MediatR;

namespace Application.Features.Nurses.Commands.DeleteShift
{
    public record DeleteShiftCommand(int NurseId, int ShiftId) : IRequest<Unit>;
}
