using Application.Common.Exceptions;
using Domain.Medicines;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Medicines.Commands.DispenseMedicine
{
    public class DispenseMedicineHandler : IRequestHandler<DispenseMedicineCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DispenseMedicineHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DispenseMedicineCommand request, CancellationToken cancellationToken)
        {
            var medicine = await _unitOfWork.Medicine.GetByIdAsync(request.MedicineId);

            if (medicine == null)
                throw new NotFoundException(nameof(Medicine), request.MedicineId);
            medicine.DispenseMedicine(request.Quantity, request.PtientId, request.PharmacistId);
            await _unitOfWork.CompleteAsync();
            return medicine.Stock;
        }
    }
}
