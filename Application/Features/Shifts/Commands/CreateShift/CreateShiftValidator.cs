using FluentValidation;

namespace Application.Features.Shifts.Commands.CreateShift
{
    public class CreateShiftValidator : AbstractValidator<CreateShiftCommand>
    {
        public CreateShiftValidator
        ()
        {
            RuleFor(x => x.StaffId)
                .NotEmpty();
            RuleFor(x => x.start)
                .NotEmpty();
            RuleFor(x => x.end)
                .NotEmpty();

        }
    }
}
