using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Departments.Commands.RenameDepartment
{
    public class RenameDepartmentHandler : IRequestHandler<RenameDepartmentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RenameDepartmentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RenameDepartmentCommand request, CancellationToken ct)
        {
            var dept = await _unitOfWork.Department.GetByIdAsync(request.Id);
            if (dept == null) throw new Exception("Department not found");

            dept.Rename(request.Name);

            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }

      
    }

}
