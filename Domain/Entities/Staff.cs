using Domain.Exceptions.Schedule;

namespace Domain.Entities
{
    public abstract class Staff : CommonData
    {
        public int Id { get; private set; }
        protected readonly List<Schedule> _schedules = new();
        public IReadOnlyCollection<Schedule> Schedules =>
        _schedules.Where(s => !s.IsDeleted).ToList().AsReadOnly();

        public Schedule AddSchedule(DayOfWeek day, TimeSpan start, TimeSpan end)
        {
            var hasConflict = Schedules.Any(s =>
                s.DayOfWeek == day &&
                s.OverlapsWith(day, start, end));

            if (hasConflict)
                throw new ConflictScheduleException();

            var newShift = Schedule.Create(day, start, end);
            _schedules.Add(newShift);

            return newShift;
        }

        public void UpdateSchedule(int scheduleId, DayOfWeek day, TimeSpan start, TimeSpan end)
        {
            var schedule = Schedules.Where(s =>
                s.Id == scheduleId).FirstOrDefault();
            if (schedule == null)
                throw new ScheduleNotFoundException();

            if (_schedules.Any(s =>
            s.Id != scheduleId &&
            s.OverlapsWith(day, start, end)))
                throw new ConflictScheduleException();

            schedule.Update(day, start, end);
        }

        public void RemoveSchedule(int scheduleId)
        {
            var schedule = Schedules.Where(s =>
                s.Id == scheduleId).FirstOrDefault();
            if (schedule == null)
                throw new ScheduleNotFoundException();
            schedule.SoftDelete();
        }
    }
}
