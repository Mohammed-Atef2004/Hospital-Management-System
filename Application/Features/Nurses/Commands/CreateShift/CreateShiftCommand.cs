using MediatR;

namespace Application.Features.Nurses.Commands.CreateShift
{
    public record CreateShiftCommand(int NurseId, DayOfWeek day, TimeSpan start, TimeSpan end) : IRequest<int>;

}
