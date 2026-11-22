using FluentValidation;

namespace Application.Features.Patients.Commands.UpsertMyProfile;

public sealed class UpsertMyPatientProfileCommandValidator : AbstractValidator<UpsertMyPatientProfileCommand>
{
    public UpsertMyPatientProfileCommandValidator()
    {
        RuleFor(command => command.UserId).NotEmpty();
        RuleFor(command => command.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(command => command.LastName).NotEmpty().MaximumLength(100);
        RuleFor(command => command.Address).NotEmpty().MaximumLength(250);
        RuleFor(command => command.City).MaximumLength(100);
        RuleFor(command => command.PhoneNumber).MaximumLength(20);
    }
}