using Application.Common.Exceptions;
using Application.Features.Doctors.DTOs;
using Application.Features.Doctors.Queries.GetById;
using Application.Features.Medicines.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                throw new NotFoundException("This Medicine not found.");
            }
            return _mapper.Map<MedicineDTO>(medicine);
        }
    }

}
