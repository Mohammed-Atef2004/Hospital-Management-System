using MediatR;

namespace Application.Features.Nurses.Commands.AddNurse
{
    public record AddNurseCommand(string FirstName, string LastName, string Address, string City, string PhoneNumber)
        : IRequest<int>
    {

    }
}
