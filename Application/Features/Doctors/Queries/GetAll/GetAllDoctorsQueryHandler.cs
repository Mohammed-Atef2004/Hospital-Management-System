using Application.Features.Doctors.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Doctors.Queries.GetAll
{
    public class GetAllDoctorsQueryHandler: IRequestHandler<GetAllDoctorsQuery, List<DoctorDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllDoctorsQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<DoctorDto>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {
            var doctors = await _unitOfWork.Doctor.GetWithDepartmentsAsync(request.IncludeInactive, cancellationToken);
            return _mapper.Map<List<DoctorDto>>(doctors);
        }
    }
}
