using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.CorrectName
{
    public record CorrectDoctorNameCommand(
    int DoctorId,
    string FirstName,
    string LastName
    ) : IRequest;
}
