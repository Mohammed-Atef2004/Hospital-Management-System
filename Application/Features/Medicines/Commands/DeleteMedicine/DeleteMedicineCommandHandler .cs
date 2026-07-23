using Application.Common.Exceptions;
using Domain.Medicines;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Medicines.Commands.DeleteMedicine
{
    public class DeleteMedicineCommandHandler : IRequestHandler<DeleteMedicineCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMedicineCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteMedicineCommand request, CancellationToken cancellationToken)
        {
            var medicine = await _unitOfWork.Medicine.GetByIdAsync(request.Id);

            if (medicine == null)
                throw new NotFoundException(nameof(Medicine), request.Id);

            medicine.SoftDelete(medicine.DeletedBy);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
