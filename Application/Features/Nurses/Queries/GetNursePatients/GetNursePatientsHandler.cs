using Application.Features.Nurses.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Nurses.Queries.GetNursePatients
{
    public class GetNursePatientsHandler : IRequestHandler<GetNursePatientsQuery, List<PatientDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetNursePatientsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<PatientDTO>> Handle(GetNursePatientsQuery request, CancellationToken cancellationToken)
        {
            var patients = await _unitOfWork.Patient.GetAllAsync(p => !p.IsDeleted && p.NurseId == request.NurseId);
            return _mapper.Map<List<PatientDTO>>(patients);
        }
    }
}
