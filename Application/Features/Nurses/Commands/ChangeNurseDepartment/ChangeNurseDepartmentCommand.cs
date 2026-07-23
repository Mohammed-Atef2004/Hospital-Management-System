using MediatR;

namespace Application.Features.Nurses.Commands.ChangeNurseDepartment
{
    public record ChangeNurseDepartmentCommand
    (int nurseId, int departmentId) : IRequest<Unit>;
}
