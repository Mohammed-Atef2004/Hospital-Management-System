using Domain.Doctors;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.Register
{
    public class RegisterNewDoctorCommandHandler : IRequestHandler<RegisterNewDoctorCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegisterNewDoctorCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(RegisterNewDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = Doctor.Create(
               firstName: request.FirstName,
                lastName: request.LastName,
                address:request.Address,
                city:request.City,
                specialization:request.Specialization);
            await _unitOfWork.Doctor.AddAsync(doctor);
            await _unitOfWork.CompleteAsync();
            return doctor.Id;
        }
    }
}
