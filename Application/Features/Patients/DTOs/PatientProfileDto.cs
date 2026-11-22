namespace Application.Features.Patients.DTOs;

public sealed record PatientProfileDto(
    int Id, string FirstName, string LastName, string Address, string? City, string? PhoneNumber);