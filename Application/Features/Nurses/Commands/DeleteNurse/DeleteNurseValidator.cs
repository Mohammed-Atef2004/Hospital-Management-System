using FluentValidation;

namespace Application.Features.Nurses.Commands.DeleteNurse
{
    internal class DeleteNurseValidator :
    AbstractValidator<DeleteNurseCommand>
    {
        public DeleteNurseValidator()
        {
            RuleFor(x => x.NurseId)
                .NotEmpty().WithMessage("Nurse Id is required");

        }
    }
}
