using Domain.personnel;
using Domain.Schedules.Exceptions;
using Domain.SharedKernel;

namespace Domain.Tests.Personnel
{

    internal sealed class TestStaff : Staff
    {
        public TestStaff() : base() { }
    }

    public class StaffTests
    {
        private static readonly DateTime BaseStart = new(2026, 7, 23, 8, 0, 0, DateTimeKind.Utc);

        [Fact]
        public void AddShift_ShouldAddShift_WhenNoConflict()
        {

            var staff = new TestStaff();
            var shift = staff.AddShift(BaseStart, BaseStart.AddHours(8));

            Assert.NotNull(shift);
            Assert.Contains(shift, staff.Shifts);
        }

        [Fact]
        public void AddShift_ShouldThrow_WhenOverlapsWithExistingShift()
        {
            var staff = new TestStaff();
            staff.AddShift(BaseStart, BaseStart.AddHours(8));
            var act = () => staff.AddShift(BaseStart.AddHours(4), BaseStart.AddHours(12));
            Assert.Throws<ShiftConflictException>(act);
        }

        [Fact]
        public void AddShift_ShouldSucceed_WhenNewShiftStartsExactlyWhenPreviousEnds()
        {
            var staff = new TestStaff();
            staff.AddShift(BaseStart, BaseStart.AddHours(8));
            var secondShift = staff.AddShift(BaseStart.AddHours(8), BaseStart.AddHours(12));

            Assert.Equal(2, staff.Shifts.Count);
            Assert.Equal(BaseStart.AddHours(8), secondShift.StartDateTime);
        }

        [Fact]
        public void AddShift_ShouldSucceed_WhenPreviousOverlappingShiftWasRemoved()
        {
            var staff = new TestStaff();
            var firstShift = staff.AddShift(BaseStart, BaseStart.AddHours(8));
            staff.RemoveShift(firstShift.Id);

            var secondShift = staff.AddShift(BaseStart, BaseStart.AddHours(8));

            Assert.Equal(2, staff.Shifts.Count);
            Assert.True(secondShift.IsActive);
        }

        [Fact]
        public void AddShift_ShouldThrow_WhenTimeRangeIsInvalid()
        {
            var staff = new TestStaff();

            var act = () => staff.AddShift(BaseStart, BaseStart.AddHours(-1));

            Assert.ThrowsAny<DomainException>(act);
        }

        [Fact]
        public void UpdateShift_ShouldUpdateTimes_WhenValidAndNoConflict()
        {
            var staff = new TestStaff();
            var shift = staff.AddShift(BaseStart, BaseStart.AddHours(8));
            var newStart = BaseStart.AddDays(1);
            var newEnd = newStart.AddHours(6);

            staff.UpdateShift(shift.Id, newStart, newEnd);

            Assert.Equal(newStart, shift.StartDateTime);
            Assert.Equal(newEnd, shift.EndDateTime);
        }

        [Fact]
        public void UpdateShift_ShouldThrow_WhenShiftDoesNotExist()
        {
            var staff = new TestStaff();

            var act = () => staff.UpdateShift(999, BaseStart, BaseStart.AddHours(4));
            Assert.ThrowsAny<DomainException>(act);
        }

        [Fact]
        public void UpdateShift_ShouldThrow_WhenConflictsWithAnotherShift()
        {
            var staff = new TestStaff();
            var shift1 = staff.AddShift(BaseStart, BaseStart.AddHours(4));
            staff.AddShift(BaseStart.AddHours(5), BaseStart.AddHours(9));

            var act = () => staff.UpdateShift(shift1.Id, BaseStart, BaseStart.AddHours(6));
            Assert.Throws<ShiftConflictException>(act);
        }

        [Fact]
        public void UpdateShift_ShouldNotConflictWithItself()
        {

            var staff = new TestStaff();
            var shift = staff.AddShift(BaseStart, BaseStart.AddHours(4));

            var act = () => staff.UpdateShift(shift.Id, BaseStart, BaseStart.AddHours(5));

            var exception = Record.Exception(act);
            Assert.Null(exception);
        }

        [Fact]
        public void RemoveShift_ShouldSoftDeleteAndDeactivateShift()
        {
            var staff = new TestStaff();
            var shift = staff.AddShift(BaseStart, BaseStart.AddHours(8));
            staff.RemoveShift(shift.Id);

            Assert.False(shift.IsActive);
            Assert.True(shift.IsDeleted);
        }

        [Fact]
        public void RemoveShift_ShouldThrow_WhenShiftDoesNotExist()
        {
            var staff = new TestStaff();
            var act = () => staff.RemoveShift(999);
            Assert.ThrowsAny<DomainException>(act);
        }
    }
}
