using Application.Features.Appointments.Commands.Create;
using FluentValidation;

public class CreateAppointmentCommandValidator
    : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentCommandValidator()
    {
        RuleFor(x => x.AppointmentDate)
            .GreaterThan(DateTime.UtcNow);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.PatientId)
            .GreaterThan(0);

        RuleFor(x => x.DoctorId)
            .GreaterThan(0);
    }
}