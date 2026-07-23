using Domain.Entities;
using Domain.Schedules.Exceptions;
using Domain.Shifts;

namespace Domain.personnel

{
    public abstract class Staff : CommonData
    {
        public int Id { get; private set; }

        protected readonly List<Shift> _shifts = new();

        protected Staff() { }

        protected Staff(int id)
        {
            Id = id;
        }

        public IReadOnlyCollection<Shift> Shifts => _shifts.AsReadOnly();

        public Shift AddShift(DateTime start, DateTime end)
        {

            var hasConflict = _shifts.Any(s => !s.IsDeleted && s.OverlapsWith(start, end));

            if (hasConflict)
                throw new ShiftConflictException();

            var newShift = Shift.Create(Id, start, end);
            _shifts.Add(newShift);

            return newShift;
        }

        public void UpdateShift(int shiftId, DateTime start, DateTime end)
        {
            var shift = _shifts.FirstOrDefault(s => s.Id == shiftId && !s.IsDeleted);
            if (shift == null)
                throw new ShiftNotFoundException();

            var hasConflict = _shifts.Any(s =>
                s.Id != shiftId &&
                !s.IsDeleted &&
                s.OverlapsWith(start, end));

            if (hasConflict)
                throw new ShiftConflictException();

            shift.Update(start, end);
        }

        public virtual void RemoveShift(int shiftId)
        {
            var shift = _shifts.FirstOrDefault(s => s.Id == shiftId && !s.IsDeleted);
            if (shift == null)
                throw new ShiftNotFoundException();

            shift.SoftDelete();
            shift.Deactivate();
        }
    }
}