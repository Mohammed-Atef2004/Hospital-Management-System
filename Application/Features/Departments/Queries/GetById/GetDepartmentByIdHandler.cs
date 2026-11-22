using Application.Common.Exceptions;
using Application.Features.Departments.DTOs;
using AutoMapper;
using Domain.Departments;
using Domain.Repositories;
using MediatR;

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
            var dept = await _unitOfWork.Department.GetFirstOrDefaultAsync(d => d.Id == request.Id && !d.IsDeleted)
                ?? throw new NotFoundException(nameof(Department), request.Id);

            return new DepartmentDto(dept.Id, dept.Name, dept.Description, dept.IsActive);
        }

    }

}
