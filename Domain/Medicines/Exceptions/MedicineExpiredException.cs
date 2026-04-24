using Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Medicines.Exceptions
{
    public class MedicineExpiredException: DomainException
    {
        public MedicineExpiredException(): base ("Can not use expired medicine.") { }
    }
}
