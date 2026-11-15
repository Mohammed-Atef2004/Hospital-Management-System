using Application.Common;
using MediatR;

namespace Application.Features.Nurses.Commands.ChangeNurseDepartment
{
    public record ChangeNurseDepartmentCommand
    (int nurseId, int departmentId) : ICommandRequest<Unit>;
}
