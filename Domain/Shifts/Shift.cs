using Domain.Entities;
using Domain.SharedKernel;

namespace Domain.Shifts
{
    public class Shift : CommonData
    {
        public int Id { get; private set; }
        public int StaffId { get; private set; }

        public DateTime StartDateTime { get; private set; }
        public DateTime EndDateTime { get; private set; }
        public bool IsActive { get; private set; } = true;

        private Shift() { }

        public static Shift Create(int staffId, DateTime start, DateTime end)
        {
            ValidateTimeRange(start, end);

            return new Shift
            {
                StaffId = staffId,
                StartDateTime = start,
                EndDateTime = end
            };
        }

        public void Update(DateTime newStart, DateTime newEnd)
        {
            ValidateTimeRange(newStart, newEnd);

            StartDateTime = newStart;
            EndDateTime = newEnd;
            UpdatedAt = DateTime.UtcNow;
        }

        private static void ValidateTimeRange(DateTime start, DateTime end)
        {
            if (start >= end)
                throw new DomainException("Shift end time must be chronologically after the start time.");

            if ((end - start).TotalHours >= 24)
                throw new DomainException("A single shift duration cannot exceed 24 continuous hours.");
        }

        public bool OverlapsWith(DateTime newStart, DateTime newEnd)
        {
            if (!IsActive) return false;

            return StartDateTime < newEnd && EndDateTime > newStart;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}