using Application.Features.Departments.DTOs;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Departments.Queries.GetAll
{
    public class GetAllDepartmentsHandler
    : IRequestHandler<GetAllDepartmentsQuery, List<DepartmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllDepartmentsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken ct)
        {
            var departments = await _unitOfWork.Department.GetAllAsync(d => !d.IsDeleted);

            return departments.Select(d =>
                new DepartmentDto(d.Id, d.Name, d.Description, d.IsActive)).ToList();
        }
    }

}
