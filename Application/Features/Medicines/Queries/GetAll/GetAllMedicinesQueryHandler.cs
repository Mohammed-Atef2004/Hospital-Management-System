using Application.Features.Doctors.DTOs;
using Application.Features.Doctors.Queries.GetAll;
using Application.Features.Medicines.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medicines.Queries.GetAll
{
    public class GetAllMedicinesQueryHandler : IRequestHandler<GetAllMedicinesQuery, List<MedicineDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllMedicinesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<MedicineDTO>> Handle(GetAllMedicinesQuery request, CancellationToken cancellationToken)
        {
            var medicines = await _unitOfWork.Medicine.GetAllAsync();
            return _mapper.Map<List<MedicineDTO>>(medicines);
        }
    }
}
