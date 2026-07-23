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
        private readonly IMapper _mapper;

        public GetDepartmentByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DepartmentDto> Handle(GetDepartmentByIdQuery request, CancellationToken ct)
        {
            var dept = await _unitOfWork.Department.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Department), request.Id);

            return _mapper.Map<DepartmentDto>(dept);
        }

    }

}
