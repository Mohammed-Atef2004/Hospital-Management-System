using Application.Features.Departments.DTOs;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Departments.Queries.GetById
{
    public class GetDepartmentByIdHandler
    : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDepartmentByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DepartmentDto> Handle(GetDepartmentByIdQuery request, CancellationToken ct)
        {
            var d = await _unitOfWork.Department.GetByIdAsync(request.Id);
            if (d == null) return null;

            return new DepartmentDto(d.Id, d.Name, d.Description, d.IsActive);
        }
    }

}
