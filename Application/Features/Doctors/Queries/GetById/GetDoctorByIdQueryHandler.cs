using Application.Common.Exceptions;
using Application.Features.Doctors.DTOs;
using AutoMapper;
using Domain.Doctors;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Doctors.Queries.GetById
{
    public class GetDoctorByIdQueryHandler : IRequestHandler<GetDoctorByIdQuery, DoctorDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetDoctorByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<DoctorDto> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.Doctor.GetByIdAsync(request.Id);
            if (doctor == null)
            {
                throw new NotFoundException(nameof(Doctor), request.Id);
            }
            return _mapper.Map<DoctorDto>(doctor);
        }
    }
}
