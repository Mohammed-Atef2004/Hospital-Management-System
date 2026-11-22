using Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Doctors.Exceptions
{
    public sealed class InvalidSpecializationException : DomainException
    {
        public InvalidSpecializationException(string? message = null)
            : base(message ?? "Specialization cannot be empty.") { }
    }
}
