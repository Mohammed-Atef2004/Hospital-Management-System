using MediatR;

namespace Application.Features.Nurses.Commands.UpdateNurse
{
    public record UpdateNurseCommand
        (int NurseId, string Address, string City, string PhoneNumber, int DeparId)
        : IRequest<Unit>
    {
    }
}
