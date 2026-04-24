using Application.Features.Medicines.DTOs;
using Application.Features.Medicines.Queries.GetAll;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medicines.Queries.GetExpiredMedicines
{
    public class GetExpiredMedicinesQueryHandler : IRequestHandler<GetExpiredMedicinesQuery, List<MedicineDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetExpiredMedicinesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<MedicineDTO>> Handle(GetExpiredMedicinesQuery request, CancellationToken cancellationToken)
        {
            var medicines = await _unitOfWork.Medicine.GetExpiredMedicinesAsync();
            return _mapper.Map<List<MedicineDTO>>(medicines);
        }
    }
}
