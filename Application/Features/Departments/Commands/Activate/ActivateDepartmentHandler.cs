using Application.Common.Exceptions;
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
            var dept = await _unitOfWork.Department.GetFirstOrDefaultAsync(d => d.Id == request.Id && !d.IsDeleted)
                ?? throw new NotFoundException("Department", request.Id);

            dept.Activate();
            await _unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }

}
