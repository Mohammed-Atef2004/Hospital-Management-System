using Application.Common.Exceptions;
using Application.Features.Shifts.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Shifts.Queries.GetShiftById
{
    public class GetShiftByIdHandler : IRequestHandler<GetShiftByIdQuery, ShiftDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetShiftByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ShiftDTO> Handle(GetShiftByIdQuery request, CancellationToken cancellationToken)
        {
            var shift = await _unitOfWork.Shift.GetByIdAsync(request.shiftId);
            if (shift == null)
            {
                throw new NotFoundException(nameof(shift), request.shiftId);
            }
            return _mapper.Map<ShiftDTO>(shift);
        }
    }
}
