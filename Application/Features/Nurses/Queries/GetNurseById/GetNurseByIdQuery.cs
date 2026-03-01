using Application.Features.Nurses.DTOs;
using MediatR;

namespace Application.Features.Nurses.Queries.GetNurseById
{
    public record GetNurseByIdQuery(int Id) :
    IRequest<NurseDTO>;
}
