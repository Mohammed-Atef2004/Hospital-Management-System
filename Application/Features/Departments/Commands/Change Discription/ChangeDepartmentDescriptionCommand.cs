using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Departments.Commands.Change_Discription
{
    public record ChangeDepartmentDescriptionCommand(int Id, string Description) : IRequest;

}
