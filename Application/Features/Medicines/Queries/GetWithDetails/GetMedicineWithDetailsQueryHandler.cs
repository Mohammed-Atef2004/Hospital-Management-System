using Application.Common.Exceptions;
using Application.Features.Medicines.DTOs;
using Application.Features.Medicines.Queries.GetById;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medicines.Queries.GetWithDetails
{
    public class GetMedicineWithDetailsQueryHandler : IRequestHandler<GetMedicineWithDetailsQuery,MedicineDetailsDTO>
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
                throw new NotFoundException("This Medicine not found.");
            }
            var medicineDetailsDto = _mapper.Map<MedicineDetailsDTO>(medicine);

            return medicineDetailsDto;
        }
    }

}
