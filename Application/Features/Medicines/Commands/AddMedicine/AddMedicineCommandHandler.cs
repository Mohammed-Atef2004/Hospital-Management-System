using Application.Common.Exceptions;
using Application.Features.Doctors.Commands.UpdateSpecialization;
using Domain.Entities;
using Domain.Medicines;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medicines.Commands.AddMedicine
{
    public class AddMedicineCommandHandler : IRequestHandler<AddMedicineCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddMedicineCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(AddMedicineCommand request, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Medicine.ExistsByNameAsync(request.Name))
                throw new ConflictException($"Medicine '{request.Name}' already exists.");
            var medicine = new Medicine(
                           name: request.Name,
                           price: request.Price,
                           quantity: request.Stock,
                           expiredate: request.ExpiryDate,
                           CreatedBy: request.CreatedBy
                       );
            await _unitOfWork.Medicine.AddAsync(medicine);
            await _unitOfWork.CompleteAsync();
            return medicine.Id;
        }
    }
}
