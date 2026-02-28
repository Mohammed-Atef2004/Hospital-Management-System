using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Medicine
{
    public class InvalidMedicineNameException:DomainException
    {
        public InvalidMedicineNameException() : base("Medicine name can not be empty."){}
    }
}
