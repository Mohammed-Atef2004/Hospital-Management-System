using Application.Common.Exceptions;
using Domain.Departments;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Departments.Commands.Create
{
    public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateDepartmentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateDepartmentCommand request, CancellationToken ct)
        {
            if (await _unitOfWork.Department.ExistsByNameAsync(request.Name))
                throw new ConflictException($"Department '{request.Name}' already exists.");

            var dept = Department.Register(request.Name, request.Description);

            await _unitOfWork.Department.AddAsync(dept);
            await _unitOfWork.CompleteAsync();

            return dept.Id;
        }


    }


}
