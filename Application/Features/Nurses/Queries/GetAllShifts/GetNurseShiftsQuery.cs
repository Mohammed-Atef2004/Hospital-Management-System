using Application.Features.Nurses.DTOs;
using MediatR;

namespace Application.Features.Nurses.Queries.GetAllShifts
{
    public record GetNurseShiftsQuery(int NurseId) : IRequest<List<ShiftDTO>>;

}
