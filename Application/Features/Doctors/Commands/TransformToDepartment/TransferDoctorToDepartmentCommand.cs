using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.TransformToDepartment
{
    public record TransferDoctorToDepartmentCommand(
     int DoctorId,
     int TargetDepartmentId
    ) : IRequest;
}
