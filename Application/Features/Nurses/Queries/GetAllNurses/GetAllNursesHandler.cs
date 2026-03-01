using Application.Features.Nurses.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Nurses.Queries.GetAllNurses
{
    internal class GetAllNursesHandler :
        IRequestHandler<GetAllNursesQuery, List<NurseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllNursesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<NurseDTO>> Handle(GetAllNursesQuery request, CancellationToken cancellationToken)
        {
            var nurses = await _unitOfWork.Nurse.GetAllAsync(n => !n.IsDeleted);
            return _mapper.Map<List<NurseDTO>>(nurses);

        }
    }
}
