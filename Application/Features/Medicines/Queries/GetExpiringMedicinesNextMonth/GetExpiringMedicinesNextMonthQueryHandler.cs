using Application.Features.Medicines.DTOs;
using Application.Features.Medicines.Queries.GetExpiredMedicines;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medicines.Queries.GetExpiringMedicinesNextMonth
{
    public class GetExpiringMedicinesNextMonthQueryHandler : IRequestHandler<GetExpiringMedicinesNextMonthQuery, List<MedicineDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetExpiringMedicinesNextMonthQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<MedicineDTO>> Handle(GetExpiringMedicinesNextMonthQuery request, CancellationToken cancellationToken)
        {
            var medicines = await _unitOfWork.Medicine.GetMedicinesExpiringInNextMonthAsync();
            return _mapper.Map<List<MedicineDTO>>(medicines);
        }
    }
}
