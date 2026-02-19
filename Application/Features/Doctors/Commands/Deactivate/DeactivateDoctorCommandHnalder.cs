using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.Deactivate
{
    public class DeactivateDoctorCommandHnalder: IRequestHandler<DeactivateDoctorCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeactivateDoctorCommandHnalder(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeactivateDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.Doctor.GetByIdAsync(request.DoctorId);
            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }
            doctor.Deactivate();
            _unitOfWork.Doctor.Update(doctor);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }   
}
