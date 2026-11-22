using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Authentication.Commands.LinkStaffProfile;

public sealed class LinkStaffProfileCommandHandler : IRequestHandler<LinkStaffProfileCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public LinkStaffProfileCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(LinkStaffProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.User.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException("User", request.UserId);
        var staff = await _unitOfWork.Staff.GetByIdAsync(request.StaffId)
            ?? throw new NotFoundException("Staff", request.StaffId);

        var result = user.LinkStaffProfile(staff);
        if (result.IsFailure)
            throw new BadRequestException(result.Error.Message);

        await _unitOfWork.CompleteAsync();
        return Unit.Value;
    }
}
