using Application.Features.Shifts.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Shifts.Queries.GetAvailableShifts
{
    public class GetAvailableShiftsHandler : IRequestHandler<GetAvailableShiftsQuery, List<ShiftDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAvailableShiftsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<ShiftDTO>> Handle(GetAvailableShiftsQuery request, CancellationToken cancellationToken)
        {
            var shifts = await _unitOfWork.Shift.GetShiftsInTimeRangeAsync(request.staffId, request.start, request.end, cancellationToken);

            return _mapper.Map<List<ShiftDTO>>(shifts);
        }
    }
}
