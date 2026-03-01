using FluentValidation;
namespace Application.Features.Nurses.Commands.CreateShift
{
    public class CreateShiftValidator :
    AbstractValidator<CreateShiftCommand>
    {
        public CreateShiftValidator()
        {
            RuleFor(x => x.NurseId)
                .NotEmpty().WithMessage("Nurse Id is required");

            RuleFor(x => x.day)
                .NotEmpty().WithMessage("Day is required");

            RuleFor(x => x.start)
                .NotEmpty().WithMessage("Start Time is required");

            RuleFor(x => x.end)
                .NotEmpty()
                .WithMessage("End Time is required");

        }
    }
}
