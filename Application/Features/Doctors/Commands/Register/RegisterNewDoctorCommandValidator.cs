using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.Register
{
    public class RegisterNewDoctorCommandValidator : AbstractValidator<RegisterNewDoctorCommand>
    {
        public RegisterNewDoctorCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");
            RuleFor(x => x.City).NotEmpty().WithMessage("City is required.");
            RuleFor(x => x.Specialization).NotEmpty().WithMessage("Specialization is required.");
        }

    }
}
