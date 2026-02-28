using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medicines.Commands.UpdateMedicine
{
    public record UpdateMedicineCommand(
    int Id,
    string? Name,
    decimal? Price,
    DateTime? ExpiryDate,
    string? UpdatedBy
) : IRequest;
}
