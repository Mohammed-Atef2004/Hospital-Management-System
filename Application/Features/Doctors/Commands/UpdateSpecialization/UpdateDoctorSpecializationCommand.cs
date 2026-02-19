using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.UpdateSpecialization
{
    public record UpdateDoctorSpecializationCommand(
    int DoctorId,
    string NewSpecialization
    ) : IRequest;
}
