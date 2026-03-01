using Application.Features.Nurses.DTOs;
using MediatR;

namespace Application.Features.Nurses.Queries.GetAllNurses
{
    public record GetAllNursesQuery() : IRequest<List<NurseDTO>>;

}
