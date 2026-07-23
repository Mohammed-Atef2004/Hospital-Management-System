using MediatR;

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
