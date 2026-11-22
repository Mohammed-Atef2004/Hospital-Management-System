using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Departments.Commands.Change_Discription;

public class ChangeDepartmentDescriptionHandler : IRequestHandler<ChangeDepartmentDescriptionCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangeDepartmentDescriptionHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(ChangeDepartmentDescriptionCommand request, CancellationToken cancellationToken)
    {
        var department = await _unitOfWork.Department.GetFirstOrDefaultAsync(
            item => item.Id == request.Id && !item.IsDeleted)
            ?? throw new NotFoundException("Department", request.Id);

        department.ChangeDescription(request.Description);
        await _unitOfWork.CompleteAsync();
        return Unit.Value;
    }
}