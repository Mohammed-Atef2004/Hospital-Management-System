using Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Doctors.Exceptions
{
    public sealed class DoctorNotAvailableException : DomainException
    {
        public DoctorNotAvailableException(DateTime date)
            : base($"Doctor is not available at {date}.") { }
    }
}
