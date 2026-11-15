using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Departments.Commands.Create
{
    public record CreateDepartmentCommand(string Name, string Description) : ICommandRequest<int>;

}
