using Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Medicines.Exceptions
{
    public class InvalidExpiryDateException : DomainException
    {
        public InvalidExpiryDateException() : base("Expiry date must be in the future.") { }
    }
}
