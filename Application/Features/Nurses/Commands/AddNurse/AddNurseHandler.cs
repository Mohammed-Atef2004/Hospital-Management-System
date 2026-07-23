using Domain.Nurses;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Nurses.Commands.AddNurse
{
    public class AddNurseHandler :
        IRequestHandler<AddNurseCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddNurseHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(AddNurseCommand request, CancellationToken cancellationToken)
        {
            var nurse = Nurse.Register(request.FirstName, request.LastName, request.Address, request.City, request.PhoneNumber, request.isHead);

            await _unitOfWork.Nurse.AddAsync(nurse);
            await _unitOfWork.CompleteAsync();

            return nurse.Id;
        }
    }
}

