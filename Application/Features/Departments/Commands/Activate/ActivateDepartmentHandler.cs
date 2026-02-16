using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Departments.Commands.Activate
{
    public class ActivateDepartmentHandler : IRequestHandler<ActivateDepartmentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ActivateDepartmentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ActivateDepartmentCommand request, CancellationToken ct)
        {
            var dept = await _unitOfWork.Department.GetByIdAsync(request.Id);
            if (dept == null) throw new Exception("Department not found");

            dept.Activate();
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }

}
