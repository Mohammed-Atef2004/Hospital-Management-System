using Application.Features.Medicines.DTOs;
using Application.Features.Medicines.Queries.GetExpiringMedicinesNextMonth;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medicines.Queries.GetLowStock
{
    public class GetMedicinesLowStockQueryHandler : IRequestHandler<GetMedicinesLowStockQuery, List<MedicineDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetMedicinesLowStockQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<MedicineDTO>> Handle(GetMedicinesLowStockQuery request, CancellationToken cancellationToken)
        {
            var medicines = await _unitOfWork.Medicine.GetLowStockMedicinesAsync(request.threshold);
            return _mapper.Map<List<MedicineDTO>>(medicines);
        }
    }
}
