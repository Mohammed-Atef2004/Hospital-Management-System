using FluentValidation;

namespace Application.Features.Shifts.Commands.RemoveShift
{
    public class RemoveShiftValidator : AbstractValidator<RemoveShiftCommand>
    {
        public RemoveShiftValidator
        ()
        {
            RuleFor(x => x.shiftId)
                .NotEmpty();
            RuleFor(x => x.staffId)
                .NotEmpty();
        }
    }
}
