using Domain.SharedKernel;

namespace Domain.Schedules.Exceptions
{
    internal sealed class ScheduleNotFoundException : DomainException
    {
        public ScheduleNotFoundException()
            : base("Schedule Not Found .") { }
    }
}
