using Application.Features.Shifts.DTOs;
using MediatR;

namespace Application.Features.Shifts.Queries.GetStaffShifts
{
    public record GetStaffShiftsQuery
    (int staffId) : IRequest<List<ShiftDTO>>;
}
