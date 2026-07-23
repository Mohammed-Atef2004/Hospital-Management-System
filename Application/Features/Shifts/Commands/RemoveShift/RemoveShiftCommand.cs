using MediatR;

namespace Application.Features.Shifts.Commands.RemoveShift
{
    public record RemoveShiftCommand(int shiftId, int staffId) : IRequest<Unit>;
}
