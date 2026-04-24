using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medicines.Commands.AddMedicine
{
    public record AddMedicineCommand(string Name,
    decimal Price,
    int Stock,
    DateTime ExpiryDate,
    string? CreatedBy) : IRequest<int>;
}
