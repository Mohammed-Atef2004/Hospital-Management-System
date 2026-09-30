using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Appointments.Commands.UpdateDescription
{
    public record UpdateDesciptionCommand(
        int Id,
        string Description
        ):IRequest;
    
}
