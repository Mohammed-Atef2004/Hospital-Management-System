using MediatR;

namespace Application.Features.Nurses.Commands.DeleteNurse
{
    public record DeleteNurseCommand(int NurseId) : IRequest<Unit>
    {
    }
}
