using Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tests.Doctor
{
    public class DoctorScheduleTests
    {
        [Fact]
        public void AddSchedule_DuplicateTime_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var doctor = Domain.Entities.Doctor.Create("Ahmed", "Ali", "Addr", "City", "Spec");
            var schedule = new Schedule { DayofWeek = DayOfWeek.Monday, StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(12, 0, 0) };
            doctor.AddSchedule(schedule);

            // Act
            Action act = () => doctor.AddSchedule(schedule);

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Schedule already exists for this time.");
        }
    }
}
