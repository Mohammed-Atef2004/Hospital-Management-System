using Application.Common;
using MediatR;

namespace Application.Features.Shifts.Commands.UpdateShift
{
    public record UpdateShiftCommand(int shiftId, int staffId, DateTime start, DateTime end) : ICommandRequest<Unit>;
}
