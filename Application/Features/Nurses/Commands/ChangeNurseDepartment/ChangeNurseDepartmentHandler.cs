using Application.Common.Exceptions;
using Domain.Departments;
using Domain.Interfaces.Services;
using Domain.Nurses;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Nurses.Commands.ChangeNurseDepartment
{
    public class ChangeNurseDepartmentHandler :
        IRequestHandler<ChangeNurseDepartmentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public ChangeNurseDepartmentHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(ChangeNurseDepartmentCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId
                ?? throw new UnauthorizedAccessException("User must be logged in.");

            var currentUser = await _unitOfWork.Nurse.GetByIdAsync(currentUserId);

            var isAuthorized = _currentUserService.IsInRole("Admin") || (currentUser != null && currentUser.IsHeadNurse);

            if (!isAuthorized)
            {
                throw new ForbiddenAccessException();
            }
            var nurseToUpdate = await _unitOfWork.Nurse.GetByIdAsync(request.nurseId)
                ?? throw new NotFoundException(nameof(Nurse), request.nurseId);

            if (!_currentUserService.IsInRole("Admin"))
            {
                if (currentUser!.DepartmentId != nurseToUpdate.DepartmentId)
                {
                    throw new ForbiddenAccessException();
                }
            }

            var departmentExists = await _unitOfWork.Department.GetByIdAsync(request.departmentId)
                ?? throw new NotFoundException(nameof(Department), request.departmentId);

            nurseToUpdate.UpdateDepartment(request.departmentId);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
