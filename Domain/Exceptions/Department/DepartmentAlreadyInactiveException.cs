using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Department
{
    public sealed class DepartmentAlreadyInactiveException : DomainException
    {
        public DepartmentAlreadyInactiveException(int id)
            : base($"Department with id '{id}' is already inactive.") { }
    }
}
