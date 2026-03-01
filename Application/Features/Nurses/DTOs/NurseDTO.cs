namespace Application.Features.Nurses.DTOs
{
    public record NurseDTO(string FirstName, string LastName, string Address, string City, string PhoneNumber, int? DepartementId);
}
