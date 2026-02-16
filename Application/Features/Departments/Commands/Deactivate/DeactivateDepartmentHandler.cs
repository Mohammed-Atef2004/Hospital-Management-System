using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Departments.Commands.Deactivate
{
    public class DeactivateDepartmentHandler : IRequestHandler<DeactivateDepartmentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateDepartmentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeactivateDepartmentCommand request, CancellationToken ct)
        {
            var dept = await _unitOfWork.Department.GetByIdAsync(request.Id);
            if (dept == null) throw new Exception("Department not found");

            dept.Deactivate();
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }

}
