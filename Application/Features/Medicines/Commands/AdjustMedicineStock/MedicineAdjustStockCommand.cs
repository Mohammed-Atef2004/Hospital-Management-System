using Application.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medicines.Commands.AdjustMedicineStock
{
    public record MedicineAdjustStockCommand(
    int MedicineId,
    int Quantity,
    StockOperation Operation,
    string Reason,
    //string Pharmacist
    int PharmacistId
        ) : IRequest<int>;
}
