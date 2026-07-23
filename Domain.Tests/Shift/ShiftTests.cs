using Domain.SharedKernel;
using Domain.Shifts;

namespace Domain.Tests.Shifts
{
    public class ShiftTests
    {
        private static readonly DateTime BaseStart = new(2026, 7, 23, 8, 0, 0, DateTimeKind.Utc);

        [Fact]
        public void Create_ShouldCreateShift_WithValidTimeRange()
        {

            var start = BaseStart;
            var end = BaseStart.AddHours(8);

            var shift = Shift.Create(staffId: 1, start: start, end: end);

            Assert.Equal(1, shift.StaffId);
            Assert.Equal(start, shift.StartDateTime);
            Assert.Equal(end, shift.EndDateTime);
            Assert.True(shift.IsActive);
        }

        [Fact]
        public void Create_ShouldThrow_WhenEndIsBeforeStart()
        {
            var start = BaseStart;
            var end = BaseStart.AddHours(-1);

            var act = () => Shift.Create(1, start, end);

            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void Create_ShouldThrow_WhenEndEqualsStart()
        {
            var act = () => Shift.Create(1, BaseStart, BaseStart);

            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void Create_ShouldThrow_WhenDurationExceeds24Hours()
        {
            var start = BaseStart;
            var end = BaseStart.AddHours(25);

            var act = () => Shift.Create(1, start, end);

            Assert.Throws<DomainException>(act);
        }


        [Fact]
        public void Update_ShouldChangeStartAndEndTimes_WhenValid()
        {
            var shift = Shift.Create(1, BaseStart, BaseStart.AddHours(8));
            var newStart = BaseStart.AddDays(1);
            var newEnd = newStart.AddHours(6);
            shift.Update(newStart, newEnd);

            Assert.Equal(newStart, shift.StartDateTime);
            Assert.Equal(newEnd, shift.EndDateTime);
        }

        [Fact]
        public void Update_ShouldThrow_WhenNewRangeIsInvalid()
        {

            var shift = Shift.Create(1, BaseStart, BaseStart.AddHours(8));

            var act = () => shift.Update(BaseStart, BaseStart.AddHours(-2));

            Assert.Throws<DomainException>(act);
        }


        [Fact]
        public void OverlapsWith_ShouldReturnTrue_WhenRangesOverlap()
        {

            var shift = Shift.Create(1, BaseStart, BaseStart.AddHours(8));

            var result = shift.OverlapsWith(BaseStart.AddHours(6), BaseStart.AddHours(12));
            Assert.True(result);
        }

        [Fact]
        public void OverlapsWith_ShouldReturnFalse_WhenRangesDoNotOverlap()
        {
            var shift = Shift.Create(1, BaseStart, BaseStart.AddHours(8));
            var result = shift.OverlapsWith(BaseStart.AddHours(10), BaseStart.AddHours(14));

            Assert.False(result);
        }

        [Fact]
        public void OverlapsWith_ShouldReturnFalse_WhenRangesAreAdjacent()
        {

            var shift = Shift.Create(1, BaseStart, BaseStart.AddHours(8));

            var result = shift.OverlapsWith(BaseStart.AddHours(8), BaseStart.AddHours(12));

            Assert.False(result);
        }

        [Fact]
        public void OverlapsWith_ShouldReturnFalse_WhenShiftIsInactive()
        {
            var shift = Shift.Create(1, BaseStart, BaseStart.AddHours(8));
            shift.Deactivate();

            var result = shift.OverlapsWith(BaseStart, BaseStart.AddHours(8));
            Assert.False(result);
        }

        [Fact]
        public void Deactivate_ShouldSetIsActiveToFalse()
        {
            var shift = Shift.Create(1, BaseStart, BaseStart.AddHours(8));

            shift.Deactivate();
            Assert.False(shift.IsActive);
        }


    }
}