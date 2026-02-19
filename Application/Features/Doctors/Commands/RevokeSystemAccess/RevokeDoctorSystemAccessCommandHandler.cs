using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.RevokeSystemAccess
{
    public class RevokeDoctorSystemAccessCommandHandler : IRequestHandler<RevokeDoctorSystemAccessCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RevokeDoctorSystemAccessCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(RevokeDoctorSystemAccessCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.Doctor.GetByIdAsync(request.DoctorId);
            if (doctor == null) throw new NotFoundException($"Doctor with id {request.DoctorId} not found.");
            doctor.RevokeSystemAccess();
            _unitOfWork.Doctor.Update(doctor);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
