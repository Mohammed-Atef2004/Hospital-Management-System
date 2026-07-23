using Application.Common.Exceptions;
using Domain.Doctors;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Doctors.Commands.UpdateSpecialization
{
    public class UpdateDoctorSpecializationCommandHandler : IRequestHandler<UpdateDoctorSpecializationCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateDoctorSpecializationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(UpdateDoctorSpecializationCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.Doctor.GetByIdAsync(request.DoctorId);
            if (doctor == null)
            {
                throw new NotFoundException(nameof(Doctor), request.DoctorId);
            }
            doctor.UpdateSpecialization(request.NewSpecialization);
            _unitOfWork.Doctor.Update(doctor);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
