using Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Departments.Exceptions
{
    public sealed class DepartmentAlreadyInactiveException : DomainException
    {
        public DepartmentAlreadyInactiveException(int id)
            : base($"Department with id '{id}' is already inactive.") { }
    }
}
