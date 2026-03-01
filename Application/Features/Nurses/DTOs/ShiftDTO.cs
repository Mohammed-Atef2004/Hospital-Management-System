namespace Application.Features.Nurses.DTOs
{
    public record ShiftDTO(DayOfWeek day, TimeSpan start, TimeSpan end);

}
