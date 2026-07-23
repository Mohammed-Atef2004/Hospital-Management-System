using MediatR;

namespace Application.Features.Shifts.Commands.CreateShift
{
    public record CreateShiftCommand(int StaffId, DateTime start, DateTime end) : IRequest<int>;
}
