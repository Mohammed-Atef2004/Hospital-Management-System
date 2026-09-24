using Application.Features.Appointments.Commands.Cancel;
using FluentValidation;

public class CancelAppointmentCommandValidator
    : AbstractValidator<CancelAppointmentCommand>
{
    public CancelAppointmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}