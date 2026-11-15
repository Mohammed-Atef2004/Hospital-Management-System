using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Departments.Commands.RenameDepartment
{
    public record RenameDepartmentCommand(int Id, string Name) : ICommandRequest<Unit>;

}
