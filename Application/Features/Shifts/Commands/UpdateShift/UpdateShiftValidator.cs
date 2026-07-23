using FluentValidation;

namespace Application.Features.Shifts.Commands.UpdateShift
{
    public class UpdateShiftValidator : AbstractValidator<UpdateShiftCommand>
    {
        public UpdateShiftValidator
        ()
        {
            RuleFor(x => x.shiftId)
                .NotEmpty();
            RuleFor(x => x.staffId)
                .NotEmpty();
            RuleFor(x => x.start)
                .NotEmpty();
            RuleFor(x => x.end)
                .NotEmpty();

        }
    }
}
