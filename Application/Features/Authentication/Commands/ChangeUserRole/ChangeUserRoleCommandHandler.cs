using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Authentication.Commands.ChangeUserRole;

public sealed class ChangeUserRoleCommandHandler : IRequestHandler<ChangeUserRoleCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangeUserRoleCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.User.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException("User", request.UserId);
        var result = user.ChangeRole(request.Role);
        if (result.IsFailure)
            throw new BadRequestException(result.Error.Message);

        await _unitOfWork.CompleteAsync();
        return Unit.Value;
    }
}
