using Application.Common.Exceptions;
using Application.Features.Medicines.DTOs;
using AutoMapper;
using Domain.Medicines;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Medicines.Queries.GetWithDetails
{
    public class GetMedicineWithDetailsQueryHandler : IRequestHandler<GetMedicineWithDetailsQuery, MedicineDetailsDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetMedicineWithDetailsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<MedicineDetailsDTO> Handle(GetMedicineWithDetailsQuery request, CancellationToken cancellationToken)
        {
            var medicine = await _unitOfWork.Medicine.GetMedicineWithDetailsAsync(request.Id);
            if (medicine == null)
            {
                throw new NotFoundException(nameof(Medicine), request.Id);
            }
            var medicineDetailsDto = _mapper.Map<MedicineDetailsDTO>(medicine);

            return medicineDetailsDto;
        }
    }

}
