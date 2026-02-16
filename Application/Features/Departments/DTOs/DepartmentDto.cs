using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Departments.DTOs
{
    public record DepartmentDto(int Id, string Name, string Description, bool IsActive);

}
