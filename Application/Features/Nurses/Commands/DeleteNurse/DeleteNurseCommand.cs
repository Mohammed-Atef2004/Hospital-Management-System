using Application.Common;
using MediatR;

namespace Application.Features.Nurses.Commands.DeleteNurse
{
    public record DeleteNurseCommand(int NurseId) : ICommandRequest<Unit>
    {
    }
}
