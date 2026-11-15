using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medicines.Commands.DeleteMedicine
{
    public record DeleteMedicineCommand(int Id) : ICommandRequest<Unit>;
}
