using Application.Common.Exceptions;
using Domain.Departments;
using Domain.Doctors;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Doctors.Commands.TransformToDepartment
{
    public class TransferDoctorToDepartmentCommandHandler : IRequestHandler<TransferDoctorToDepartmentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransferDoctorToDepartmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(TransferDoctorToDepartmentCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _unitOfWork.Doctor.GetByIdAsync(request.DoctorId);
            if (doctor == null) throw new NotFoundException(nameof(Doctor), request.DoctorId);
            var department = await _unitOfWork.Department.GetByIdAsync(request.TargetDepartmentId);
            if (department == null) throw new NotFoundException(nameof(Department),
            request.TargetDepartmentId);
            doctor.TransferToDepartment(department);
            _unitOfWork.Doctor.Update(doctor);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
