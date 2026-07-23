using Domain.SharedKernel;

namespace Domain.Schedules.Exceptions
{
    internal sealed class ShiftNotFoundException : DomainException
    {
        public ShiftNotFoundException()
            : base("Shift Not Found .") { }
    }
}
