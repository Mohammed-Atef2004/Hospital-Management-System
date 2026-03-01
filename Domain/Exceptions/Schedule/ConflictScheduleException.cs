namespace Domain.Exceptions.Schedule
{
    public sealed class ConflictScheduleException : DomainException
    {
        public ConflictScheduleException()
            : base("Schedule conflict .") { }
    }
}
