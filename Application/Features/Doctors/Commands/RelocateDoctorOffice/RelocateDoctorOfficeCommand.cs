using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.RelocateDoctorOffice
{
    public record RelocateDoctorOfficeCommand(
    int DoctorId,
    string NewAddress,
    string NewCity
    ) : IRequest;
}
