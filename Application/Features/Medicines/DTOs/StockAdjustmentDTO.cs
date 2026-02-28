using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medicines.DTOs
{
    public record StockAdjustmentDTO 
        (
        int Quantity,
        string Reason,
        int PharmacistId,
        DateTime Date
        );
}
