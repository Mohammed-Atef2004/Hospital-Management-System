using FluentValidation;

namespace Application.Features.Nurses.Commands.UpdateNurse
{
    public class UpdateNurseValidator :
    AbstractValidator<UpdateNurseCommand>
    {
        public UpdateNurseValidator()
        {
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Length(11)
                .WithMessage("Phone Number is required with 11 digit");

        }
    }
}
