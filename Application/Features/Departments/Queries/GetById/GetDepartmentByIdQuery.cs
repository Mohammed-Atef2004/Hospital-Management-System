using Application.Features.Departments.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Departments.Queries.GetById
{
    public record GetDepartmentByIdQuery(int Id) : IRequest<DepartmentDto>;

}
