using Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Medicines.Exceptions
{
    public class InvalidMedicinePriceException : DomainException
    {
        public InvalidMedicinePriceException() : base("Medicine price must be grater than zero.") { }
    }
}
