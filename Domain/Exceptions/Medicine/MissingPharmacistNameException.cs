using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Medicine
{
    public class MissingPharmacistNameException : DomainException
    {
        public MissingPharmacistNameException() : base("Pharmacist name is required.") { }
    }
}
