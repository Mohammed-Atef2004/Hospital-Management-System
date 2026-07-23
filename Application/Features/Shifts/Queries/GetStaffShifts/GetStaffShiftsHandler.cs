using Application.Features.Shifts.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Shifts.Queries.GetStaffShifts
{
    public class GetStaffShiftsHandler : IRequestHandler<GetStaffShiftsQuery, List<ShiftDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetStaffShiftsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<ShiftDTO>> Handle(GetStaffShiftsQuery request, CancellationToken cancellationToken)
        {
            var shifts = await _unitOfWork.Shift.GetShiftsByStaffIdAsync(request.staffId, cancellationToken);

            return _mapper.Map<List<ShiftDTO>>(shifts);
        }
    }
}
