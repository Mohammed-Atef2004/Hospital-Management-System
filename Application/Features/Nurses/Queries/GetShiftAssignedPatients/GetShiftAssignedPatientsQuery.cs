using Application.Features.Nurses.DTOs;
using MediatR;

namespace Application.Features.Nurses.Queries.GetShiftAssignedPatients
{
    public record GetShiftAssignedPatientsQuery(int ShitfId, int NurseId)
    : IRequest<List<PatientDTO>>;
}
