using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Department
{

    public sealed class DepartmentAlreadyExistsException : DomainException
    {
        public DepartmentAlreadyExistsException(string name)
            : base($"Department '{name}' already exists.") { }
    }
}
