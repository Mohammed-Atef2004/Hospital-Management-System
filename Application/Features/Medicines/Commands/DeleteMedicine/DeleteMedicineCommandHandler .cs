using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medicines.Commands.DeleteMedicine
{
    public class DeleteMedicineCommandHandler: IRequestHandler<DeleteMedicineCommand>
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
                throw new NotFoundException("Medicine not found");

            medicine.SoftDelete(medicine.DeletedBy);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
