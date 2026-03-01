using Application.Common.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Nurses.Commands.CreateShift
{
    public class CreateShiftHandler :
        IRequestHandler<CreateShiftCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateShiftHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(CreateShiftCommand request, CancellationToken cancellationToken)
        {
            var nurse = await _unitOfWork.Nurse.GetByIdAsync(request.NurseId);
            if (nurse == null)
                throw new NotFoundException($"Nurse with id {request.NurseId} not found.");

            var idSchedule = nurse.AddSchedule(request.day, request.start, request.end);
            await _unitOfWork.CompleteAsync();
            return idSchedule.Id;
        }
    }
}
