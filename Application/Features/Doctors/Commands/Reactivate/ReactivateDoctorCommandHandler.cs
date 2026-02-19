using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.Reactivate
{
    public class ReactivateDoctorCommandHandler : IRequestHandler<ReactivateDoctorCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReactivateDoctorCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(ReactivateDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.Doctor.GetByIdAsync(request.DoctorId);
            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }
            doctor.Reactivate();
            _unitOfWork.Doctor.Update(doctor);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
