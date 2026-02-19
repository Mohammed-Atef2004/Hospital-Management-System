using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Doctors
{
    public sealed class InvalidSpecializationException : DomainException
    {
        public InvalidSpecializationException()
            : base("Specialization cannot be empty.") { }
    }
}
