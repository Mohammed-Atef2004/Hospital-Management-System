using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Department
{
    public sealed class DepartmentInvalidNameException : DomainException
    {
        public DepartmentInvalidNameException()
            : base("Department name cannot be empty.") { }
    }
}
