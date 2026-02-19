using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (doctor == null) throw new NotFoundException($"Doctor with id {request.DoctorId} not found.");
            var department = await _unitOfWork.Department.GetByIdAsync(request.TargetDepartmentId);
            if (department == null) throw new NotFoundException($"Department with id {request.TargetDepartmentId} not found.");
            doctor.TransferToDepartment(department);
            _unitOfWork.Doctor.Update(doctor);
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
