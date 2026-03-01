using Application.Features.Nurses.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Nurses.Queries.GetAllShifts
{
    public class GetNurseShiftsHandler
        : IRequestHandler<GetNurseShiftsQuery, List<ShiftDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetNurseShiftsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ShiftDTO>> Handle(GetNurseShiftsQuery request, CancellationToken cancellationToken)
        {
            var schedules = await _unitOfWork.Schedule
                .GetAllAsync(s => s.StaffId == request.NurseId && s.IsDeleted == false);

            return _mapper.Map<List<ShiftDTO>>(schedules);
        }
    }
}
