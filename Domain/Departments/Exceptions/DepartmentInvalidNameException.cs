using Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Departments.Exceptions
{
    public sealed class DepartmentInvalidNameException : DomainException
    {
        public DepartmentInvalidNameException()
            : base("Department name cannot be empty.") { }
    }
}
