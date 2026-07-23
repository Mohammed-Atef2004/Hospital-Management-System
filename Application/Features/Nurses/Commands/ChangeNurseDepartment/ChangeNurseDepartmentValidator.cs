using FluentValidation;

namespace Application.Features.Nurses.Commands.ChangeNurseDepartment
{
    public class ChangeNurseDepartmentValidator :
    AbstractValidator<ChangeNurseDepartmentCommand>
    {
        public ChangeNurseDepartmentValidator()
        {
            RuleFor(x => x.nurseId)
                .NotEmpty().WithMessage("Nurse Id is required");
            RuleFor(x => x.departmentId)
                .NotEmpty();

        }
    }
}
