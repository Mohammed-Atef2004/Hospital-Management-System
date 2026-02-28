using Application.Common.Exceptions;
using Application.Enums;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medicines.Commands.AdjustMedicineStock
{
    public class MedicineAdjustStockHandler: IRequestHandler<MedicineAdjustStockCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicineAdjustStockHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(MedicineAdjustStockCommand request,CancellationToken cancellationToken)
        {
            var medicine = await _unitOfWork.Medicine.GetByIdAsync(request.MedicineId);

            if (medicine == null)
                throw new NotFoundException("Medicine not found");

            int amount;
            if (request.Operation == StockOperation.Increase)
                amount = request.Quantity;
            else
                amount = -request.Quantity;

            medicine.AdjustStock(amount,request.Reason,request.PharmacistId);

            await _unitOfWork.CompleteAsync();
            return medicine.Stock;
        }
    }
}
