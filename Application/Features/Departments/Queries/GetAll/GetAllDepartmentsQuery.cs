using Application.Features.Departments.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Departments.Queries.GetAll
{
    public record GetAllDepartmentsQuery() : IRequest<List<DepartmentDto>>;
}
