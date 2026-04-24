using Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Medicines.Exceptions
{
    public class NotEnoughStockException : DomainException
    {
        public NotEnoughStockException(): base ("Not enoygh stock available."){ }
    }
}
