using Domain.Entities;
using Domain.Exceptions.Doctors;
using FluentAssertions;
using Xunit;

namespace Domain.Tests.Doctor
{
    public class DoctorAppointmentTests
    {
        [Fact]
        public void BookAppointment_WhenDoctorNotAvailable_ShouldThrowDoctorNotAvailableException()
        {
            // Arrange
            var doctor = Entities.Doctor.Create("Ahmed", "Ali", "Addr", "City", "Spec");
            var appointmentDate = DateTime.UtcNow.AddDays(7);

            var appointment = new Appointment
            {
                AppointmentDate = appointmentDate,
                Description = "Checkup"
            };

            // Act
            Action act = () => doctor.BookAppointment(appointment, DateTime.UtcNow);

            // Assert
            act.Should().Throw<DoctorNotAvailableException>();
        }

        [Fact]
        public void BookAppointment_InPast_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var doctor = Entities.Doctor.Create("Ahmed", "Ali", "Addr", "City", "Spec");
            var pastDate = DateTime.UtcNow.AddDays(-1);

            var appointment = new Appointment
            {
                AppointmentDate = pastDate,
                Description = "Past Appointment"
            };

            // Act
            Action act = () => doctor.BookAppointment(appointment, DateTime.UtcNow);

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Cannot book an appointment in the past.");
        }

        [Fact]
        public void BookAppointment_WhenConflictExists_ShouldThrowDoctorNotAvailableException()
        {
            // Arrange
            var doctor = Entities.Doctor.Create("Ahmed", "Ali", "Addr", "City", "Spec");
            var appointmentDate = new DateTime(2026, 5, 20, 10, 0, 0); // الأربعاء مثلاً

            var schedule = new Schedule
            {
                DayofWeek = appointmentDate.DayOfWeek,
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(16, 0, 0)
            };
            doctor.AddSchedule(schedule);

            var firstAppointment = new Appointment { AppointmentDate = appointmentDate };
            doctor.BookAppointment(firstAppointment, DateTime.UtcNow);

            var secondAppointment = new Appointment { AppointmentDate = appointmentDate };

            // Act
            Action act = () => doctor.BookAppointment(secondAppointment, DateTime.UtcNow);

            // Assert
            act.Should().Throw<DoctorNotAvailableException>();
        }
    }
}