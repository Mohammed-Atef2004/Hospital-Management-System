using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.UpdateContactDetails
{
    public class UpdateDoctorContactDetailsCommandHandler : IRequestHandler<UpdateDoctorContactDetailsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateDoctorContactDetailsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateDoctorContactDetailsCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.Doctor.GetByIdAsync(request.DoctorId);
            if (doctor == null) throw new NotFoundException($"Doctor with id {request.DoctorId} not found.");
            doctor.UpdateContactDetails(request.NewPhoneNumber);
            _unitOfWork.Doctor.Update(doctor);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
