using FluentValidation;

namespace Application.Features.Nurses.Commands.UpdateShift
{
    public class UpdateShiftValidator
     : AbstractValidator<UpdateShiftCommand>
    {
        public UpdateShiftValidator()
        {
            RuleFor(x => x.NurseId)
                .NotEmpty().WithMessage("Nurse Id is required");

            RuleFor(x => x.ShiftId)
                .NotEmpty().WithMessage("Shift Id is required");

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
