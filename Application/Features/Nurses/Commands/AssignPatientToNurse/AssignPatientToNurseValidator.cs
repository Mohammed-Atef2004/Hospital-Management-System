using FluentValidation;

namespace Application.Features.Nurses.Commands.AssignPatientToNurse
{
    public class AssignPatientToNurseValidator : AbstractValidator<AssignPatientToNurseCommand>
    {
        public AssignPatientToNurseValidator
        ()
        {
            RuleFor(x => x.nurseId).NotEmpty();
            RuleFor(x => x.patientId)
                .NotEmpty();
            RuleFor(x => x.shiftId)
                .NotEmpty();

        }
    }
}
