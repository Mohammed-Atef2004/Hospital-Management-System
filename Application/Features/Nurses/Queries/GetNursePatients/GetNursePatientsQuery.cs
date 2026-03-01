using Application.Features.Nurses.DTOs;
using MediatR;

namespace Application.Features.Nurses.Queries.GetNursePatients
{
    public record GetNursePatientsQuery(int NurseId)
    : IRequest<List<PatientDTO>>;
}
