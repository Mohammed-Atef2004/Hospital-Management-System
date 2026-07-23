using FluentValidation;

namespace Application.Features.Nurses.Commands.EndPatientAssignment
{
    public class EndPatientAssignmentValidator : AbstractValidator<EndPatientAssignmentCommand>
    {
        public EndPatientAssignmentValidator
        ()
        {

            RuleFor(x => x.AssignmentId).NotEmpty();

        }
    }
}
