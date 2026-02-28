using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Medicine
{
    public class InvalidExpiryDateException : DomainException
    {
        public InvalidExpiryDateException() : base("Expiry date must be in the future.") { }
    }
}
