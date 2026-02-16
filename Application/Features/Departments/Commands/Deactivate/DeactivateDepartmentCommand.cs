using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Departments.Commands.Deactivate
{
    public record DeactivateDepartmentCommand(int Id) : IRequest;
}
