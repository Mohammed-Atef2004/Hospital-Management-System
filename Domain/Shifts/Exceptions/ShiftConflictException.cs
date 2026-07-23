using Domain.SharedKernel;

namespace Domain.Schedules.Exceptions
{
    public sealed class ShiftConflictException : DomainException
    {
        public ShiftConflictException()
            : base("Schedule conflict .") { }
    }
}
