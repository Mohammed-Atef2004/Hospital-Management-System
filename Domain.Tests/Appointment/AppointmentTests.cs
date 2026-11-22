using Domain.Appointments.Events;
using Domain.Entities;
using Domain.SharedKernel;
using FluentAssertions;

namespace Domain.Tests.Appointments;

public sealed class AppointmentTests
{
    [Fact]
    public void Create_ValidDetails_CreatesActiveAppointmentAndBookingEvent()
    {
        var appointment = Appointment.Create(
            DateTime.UtcNow.AddDays(1), "  Annual check-up  ", patientId: 7, doctorId: 12);

        appointment.IsCancelled.Should().BeFalse();
        appointment.Description.Should().Be("Annual check-up");
        appointment.PatientId.Should().Be(7);
        appointment.DoctorId.Should().Be(12);
        appointment.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<AppointmentBookedDomainEvent>();
    }

    [Theory]
    [InlineData(0, 12)]
    [InlineData(7, 0)]
    public void Create_InvalidParticipant_ThrowsDomainException(int patientId, int doctorId)
    {
        var act = () => Appointment.Create(DateTime.UtcNow.AddDays(1), "Check-up", patientId, doctorId);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Create_PastDate_ThrowsDomainException()
    {
        var act = () => Appointment.Create(DateTime.UtcNow.AddMinutes(-1), "Check-up", 7, 12);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Cancel_ThenChangeDescription_ThrowsDomainException()
    {
        var appointment = CreateAppointment();
        appointment.Cancel();

        var act = () => appointment.ChangeDescription("Updated reason");

        act.Should().Throw<DomainException>();
        appointment.IsCancelled.Should().BeTrue();
        appointment.DomainEvents.Should().ContainSingle(@event => @event is AppointmentCancelledDomainEvent);
    }

    [Fact]
    public void ChangeAppointmentDate_ValidDate_UpdatesDateAndRaisesEvent()
    {
        var appointment = CreateAppointment();
        appointment.ClearDomainEvents();
        var newDate = DateTime.UtcNow.AddDays(2);

        appointment.ChangeAppointmentDate(newDate);

        appointment.AppointmentDate.Should().Be(newDate);
        appointment.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<AppointmentRescheduledDomainEvent>();
    }

    private static Appointment CreateAppointment()
        => Appointment.Create(DateTime.UtcNow.AddDays(1), "Check-up", 7, 12);
}
