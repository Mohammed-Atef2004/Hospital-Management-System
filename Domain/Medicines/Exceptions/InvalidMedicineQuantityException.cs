using Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Medicines.Exceptions
{
    public class InvalidMedicineQuantityException :DomainException
    {
        public InvalidMedicineQuantityException() : base("Medicine quantity can not be zero or negative.") { }
    }
}
