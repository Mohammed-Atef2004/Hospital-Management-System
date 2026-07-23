using Application.Common.Exceptions;
using Application.Features.Medicines.DTOs;
using AutoMapper;
using Domain.Medicines;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Medicines.Queries.GetById
{
    public class GetMedicineByIdQueryHandler : IRequestHandler<GetMedicineByIdQuery, MedicineDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetMedicineByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<MedicineDTO> Handle(GetMedicineByIdQuery request, CancellationToken cancellationToken)
        {
            var medicine = await _unitOfWork.Medicine.GetByIdAsync(request.Id);
            if (medicine == null)
            {
                throw new NotFoundException(nameof(Medicine), request.Id);
            }
            return _mapper.Map<MedicineDTO>(medicine);
        }
    }

}
