using Application.Common;
using MediatR;

namespace Application.Features.Shifts.Commands.RemoveShift
{
    public record RemoveShiftCommand(int shiftId, int staffId) : ICommandRequest<Unit>;
}
