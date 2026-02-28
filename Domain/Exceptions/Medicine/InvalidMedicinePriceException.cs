using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Medicine
{
    public class InvalidMedicinePriceException : DomainException
    {
        public InvalidMedicinePriceException() : base("Medicine price must be grater than zero.") { }
    }
}
