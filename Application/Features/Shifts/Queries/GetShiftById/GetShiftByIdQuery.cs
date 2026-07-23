using Application.Features.Shifts.DTOs;
using MediatR;

namespace Application.Features.Shifts.Queries.GetShiftById
{
    public record GetShiftByIdQuery
    (int shiftId) : IRequest<ShiftDTO>;
}
