using Application.Common;
using MediatR;

namespace Application.Features.Nurses.Commands.UpdateNurse
{
    public record UpdateNurseCommand
        (string Address, string City, string PhoneNumber)
        : ICommandRequest<Unit>
    {
    }
}
