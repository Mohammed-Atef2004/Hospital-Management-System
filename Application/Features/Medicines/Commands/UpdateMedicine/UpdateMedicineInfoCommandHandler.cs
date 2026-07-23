using Application.Common.Exceptions;
using Domain.Medicines;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Medicines.Commands.UpdateMedicine
{
    public class UpdateMedicineCommandHandler : IRequestHandler<UpdateMedicineCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMedicineCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateMedicineCommand request, CancellationToken cancellationToken)
        {
            var medicine = await _unitOfWork.Medicine.GetByIdAsync(request.Id);

            if (medicine == null)
                throw new NotFoundException(nameof(Medicine), request.Id);

            medicine.UpdateInfo(
                request.Name,
                request.Price,
                request.ExpiryDate,
                request.UpdatedBy
            );

            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
