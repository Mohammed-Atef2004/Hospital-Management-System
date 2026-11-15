using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.Deactivate
{
    public record DeactivateDoctorCommand(
    int DoctorId
) : ICommandRequest<Unit>;
}
