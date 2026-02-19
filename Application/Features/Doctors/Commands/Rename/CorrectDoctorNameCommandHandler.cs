using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.CorrectName
{
    public class CorrectDoctorNameCommandHandler : IRequestHandler<CorrectDoctorNameCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CorrectDoctorNameCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(CorrectDoctorNameCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.Doctor.GetByIdAsync(request.DoctorId);
            if (doctor == null)
            {
                throw new NotFoundException($"Doctor with id {request.DoctorId} not found.");
            }
            doctor.CorrectName(request.FirstName, request.LastName);
            _unitOfWork.Doctor.Update(doctor);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
