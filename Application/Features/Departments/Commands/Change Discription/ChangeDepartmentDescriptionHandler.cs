using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Departments.Commands.Change_Discription
{
    public class ChangeDepartmentDescriptionHandler
    : IRequestHandler<ChangeDepartmentDescriptionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeDepartmentDescriptionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ChangeDepartmentDescriptionCommand request, CancellationToken ct)
        {
            var dept = await _unitOfWork.Department.GetByIdAsync(request.Id);
            if (dept == null) throw new Exception("Department not found");

            dept.ChangeDescription(request.Description);

            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }

}
