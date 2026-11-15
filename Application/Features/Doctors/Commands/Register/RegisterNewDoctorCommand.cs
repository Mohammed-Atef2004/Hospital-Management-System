using Application.Common;
using MediatR;

namespace Application.Features.Doctors.Commands.Register
{
    public record RegisterNewDoctorCommand(
    string FirstName,
    string LastName,
    string Address,
    string City,
    string Specialization
) : ICommandRequest<int>;
}
