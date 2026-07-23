using FluentValidation;

namespace Application.Features.Nurses.Commands.AddNurse
{
    public class AddNurseValidator : AbstractValidator<AddNurseCommand>
    {
        public AddNurseValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required");

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