using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Appointments.Commands.UpdatePatient
{
    public record UpdatePatientCommand(
        int Id,
        int PatientId):IRequest;
}
