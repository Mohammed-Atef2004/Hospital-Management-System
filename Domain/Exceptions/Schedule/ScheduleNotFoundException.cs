namespace Domain.Exceptions.Schedule
{
    internal sealed class ScheduleNotFoundException : DomainException
    {
        public ScheduleNotFoundException()
            : base("Schedule Not Found .") { }
    }
}
