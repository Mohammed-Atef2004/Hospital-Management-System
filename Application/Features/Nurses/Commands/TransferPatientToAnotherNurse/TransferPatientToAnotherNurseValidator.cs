using FluentValidation;

namespace Application.Features.Nurses.Commands.TransferPatientToAnotherNurse
{
    public class TransferPatientToAnotherNurseValidator : AbstractValidator<TransferPatientToAnotherNurseCommand>
    {
        public TransferPatientToAnotherNurseValidator
        ()
        {
            RuleFor(x => x.nurseId).NotEmpty();
            RuleFor(x => x.AssignmentId).NotEmpty();
            RuleFor(x => x.shiftId).NotEmpty();

        }
    }
}
