using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.Register
{
    public record RegisterNewDoctorCommand(
    string FirstName,
    string LastName,
    string Address,
    string City,
    string Specialization
) : IRequest<int>;
}
