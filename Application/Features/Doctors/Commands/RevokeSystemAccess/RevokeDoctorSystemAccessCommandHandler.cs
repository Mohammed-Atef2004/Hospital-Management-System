using Application.Common.Exceptions;
using Domain.Doctors;
using Domain.Repositories;
using MediatR;

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
            if (doctor == null) throw new NotFoundException(nameof(Doctor), request.DoctorId);
            // doctor.RevokeSystemAccess();
            _unitOfWork.Doctor.Update(doctor);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
