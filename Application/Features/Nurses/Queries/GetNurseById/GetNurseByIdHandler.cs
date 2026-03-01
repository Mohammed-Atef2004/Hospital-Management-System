using Application.Common.Exceptions;
using Application.Features.Nurses.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Nurses.Queries.GetNurseById
{
    public class GetNurseByIdHandler :
        IRequestHandler<GetNurseByIdQuery, NurseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetNurseByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<NurseDTO> Handle(GetNurseByIdQuery request, CancellationToken cancellationToken)
        {
            var nurse = await _unitOfWork.Nurse.GetByIdAsync(request.Id);
            if (nurse == null || nurse.IsDeleted == true)
                throw new NotFoundException($"Nurse with id {request.Id} not found.");

            return _mapper.Map<NurseDTO>(nurse);
        }
    }
}
