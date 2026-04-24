using Domain.SharedKernel;

namespace Domain.Schedules.Exceptions
{
    public sealed class ConflictScheduleException : DomainException
    {
        public ConflictScheduleException()
            : base("Schedule conflict .") { }
    }
}
