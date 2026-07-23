using Application.Features.Shifts.DTOs;
using MediatR;

namespace Application.Features.Shifts.Queries.GetAvailableShifts
{
    public record GetAvailableShiftsQuery
    (int staffId, DateTime start, DateTime end) : IRequest<List<ShiftDTO>>;
}
