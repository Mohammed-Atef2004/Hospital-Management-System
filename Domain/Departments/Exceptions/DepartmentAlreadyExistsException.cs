using Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Departments.Exceptions
{

    public sealed class DepartmentAlreadyExistsException : DomainException
    {
        public DepartmentAlreadyExistsException(string name)
            : base($"Department '{name}' already exists.") { }
    }
}
