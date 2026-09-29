using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Appointments.Commands.UpdateDoctor
{
    public record UpdateDoctorCommand(
        int Id,
        int DoctorId):IRequest;
}
