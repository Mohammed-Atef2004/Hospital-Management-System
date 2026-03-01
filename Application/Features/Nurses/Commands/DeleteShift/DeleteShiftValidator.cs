using FluentValidation;

namespace Application.Features.Nurses.Commands.DeleteShift
{
    public class DeleteShiftValidator :
    AbstractValidator<DeleteShiftCommand>
    {
        public DeleteShiftValidator()
        {
            RuleFor(x => x.NurseId)
                .NotEmpty().WithMessage("Nurse Id is required");

            RuleFor(x => x.ShiftId)
                .NotEmpty().WithMessage("Shift Id is required");


        }
    }
}
