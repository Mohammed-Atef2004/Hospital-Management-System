using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medicines.Commands.DispenseMedicine
{
    public record DispenseMedicineCommand(
        int MedicineId,
        int Quantity,
        int PharmacistId,
        int PtientId
    ): IRequest<int>;
}
