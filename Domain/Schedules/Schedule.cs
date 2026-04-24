using Domain.Entities;

namespace Domain.Schedules
{
    public class Schedule : CommonData
    {
        public int Id { get; private set; }
        public int StaffId { get; private set; }
        public Staff Staff { get; private set; }
        public DayOfWeek DayOfWeek { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
        public bool IsActive { get; private set; } = true;

        private Schedule() { }

        private Schedule(DayOfWeek dayOfWeek, TimeSpan start, TimeSpan end)
        {
            ValidateTimeRange(start, end);

            DayOfWeek = dayOfWeek;
            StartTime = start;
            EndTime = end;
        }

        public static Schedule Create(DayOfWeek dayOfWeek, TimeSpan start, TimeSpan end)
            => new Schedule(dayOfWeek, start, end);


        public void Update(DayOfWeek newDay, TimeSpan newStart, TimeSpan newEnd)
        {
            ValidateTimeRange(newStart, newEnd);

            DayOfWeek = newDay;
            StartTime = newStart;
            EndTime = newEnd;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        private void ValidateTimeRange(TimeSpan start, TimeSpan end)
        {
            if (start == end)
                throw new ArgumentException("Shift cannot be 24 hours.");
            var duration = end - start;
            if (duration <= TimeSpan.Zero)
                throw new ArgumentException("Invalid time range.");
        }

        public bool OverlapsWith(DayOfWeek newDay, TimeSpan newStart, TimeSpan newEnd)
        {
            if (DayOfWeek != newDay)
                return false;

            return StartTime < newEnd && EndTime > newStart;
        }
    }

}

