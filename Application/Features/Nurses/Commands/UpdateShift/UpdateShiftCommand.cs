using MediatR;

namespace Application.Features.Nurses.Commands.UpdateShift
{
    public record UpdateShiftCommand(int NurseId, int ShiftId, DayOfWeek day, TimeSpan start, TimeSpan end)
        : IRequest<Unit>;

}
