using Domain.Entities;

namespace Domain.Tests.Entities;

public class AppointmentTests
{
    private static DateTime FutureDate =>
        DateTime.UtcNow.AddDays(1);

    private static Appointment CreateValidAppointment(
        DateTime? appointmentDate = null,
        string? description = null,
        int patientId = 1,
        int doctorId = 1)
    {
        return Appointment.Create(
            appointmentDate ?? FutureDate,
            description ?? "Regular medical check-up",
            patientId,
            doctorId);
    }

    #region Create

    [Fact]
    public void Create_WithValidData_ShouldCreateAppointment()
    {
        // Arrange
        var appointmentDate = FutureDate;
        const string description = "Regular medical check-up";
        const int patientId = 1;
        const int doctorId = 1;

        // Act
        var appointment = Appointment.Create(
            appointmentDate,
            description,
            patientId,
            doctorId);

        // Assert
        Assert.NotNull(appointment);
        Assert.Equal(appointmentDate, appointment.AppointmentDate);
        Assert.Equal(description, appointment.Description);
        Assert.Equal(patientId, appointment.PatientId);
        Assert.Equal(doctorId, appointment.DoctorId);
        Assert.False(appointment.IsCancelled);
        Assert.Contains(appointment.DomainEvents, domainEvent => domainEvent is Domain.Appointments.Events.AppointmentBookedDomainEvent);
    }

    [Fact]
    public void Cancel_ShouldRaiseDomainEventAndPreventFurtherChanges()
    {
        var appointment = CreateValidAppointment();
        appointment.ClearDomainEvents();

        appointment.Cancel();

        Assert.Contains(appointment.DomainEvents,
            domainEvent => domainEvent is Domain.Appointments.Events.AppointmentCancelledDomainEvent);
        Assert.Throws<InvalidOperationException>(() => appointment.ChangeAppointmentDate(FutureDate.AddDays(1)));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-1, 1)]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    public void Create_WithInvalidForeignKeys_ShouldThrowArgumentOutOfRangeException(int patientId, int doctorId)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Appointment.Create(
            FutureDate, "Regular medical check-up", patientId, doctorId));
    }

    [Fact]
    public void Create_WithPastDate_ShouldThrowArgumentException()
    {
        // Arrange
        var pastDate = DateTime.UtcNow.AddMinutes(-1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            Appointment.Create(
                pastDate,
                "Regular medical check-up",
                1,
                1));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_WithInvalidDescription_ShouldThrowArgumentException(
        string description)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            Appointment.Create(
                FutureDate,
                description,
                1,
                1));
    }

    #endregion

    #region Cancel

    [Fact]
    public void Cancel_WhenAppointmentIsActive_ShouldCancelAppointment()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        // Act
        appointment.Cancel();

        // Assert
        Assert.True(appointment.IsCancelled);
    }

    [Fact]
    public void Cancel_WhenAppointmentIsAlreadyCancelled_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Cancel();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => appointment.Cancel());
    }

    #endregion

    #region ChangeDescription

    [Fact]
    public void ChangeDescription_WithValidDescription_ShouldUpdateDescription()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        // Act
        appointment.ChangeDescription("Follow-up appointment");

        // Assert
        Assert.Equal(
            "Follow-up appointment",
            appointment.Description);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void ChangeDescription_WithInvalidDescription_ShouldThrowArgumentException(
        string description)
    {
        // Arrange
        var appointment = CreateValidAppointment();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => appointment.ChangeDescription(description));
    }

    [Fact]
    public void ChangeDescription_WhenAppointmentIsCancelled_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Cancel();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => appointment.ChangeDescription("New description"));
    }

    #endregion

    #region ChangeDoctor

    [Fact]
    public void ChangeDoctor_WithValidDoctorId_ShouldUpdateDoctor()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        // Act
        appointment.ChangeDoctor(2);

        // Assert
        Assert.Equal(2, appointment.DoctorId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ChangeDoctor_WithInvalidDoctorId_ShouldThrowArgumentOutOfRangeException(
        int doctorId)
    {
        // Arrange
        var appointment = CreateValidAppointment();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(
            () => appointment.ChangeDoctor(doctorId));
    }

    [Fact]
    public void ChangeDoctor_WhenAppointmentIsCancelled_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Cancel();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => appointment.ChangeDoctor(2));
    }

    #endregion

    #region ChangePatient

    [Fact]
    public void ChangePatient_WithValidPatientId_ShouldUpdatePatient()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        // Act
        appointment.ChangePatient(2);

        // Assert
        Assert.Equal(2, appointment.PatientId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ChangePatient_WithInvalidPatientId_ShouldThrowArgumentOutOfRangeException(
        int patientId)
    {
        // Arrange
        var appointment = CreateValidAppointment();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(
            () => appointment.ChangePatient(patientId));
    }

    [Fact]
    public void ChangePatient_WhenAppointmentIsCancelled_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Cancel();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => appointment.ChangePatient(2));
    }

    #endregion

    #region ChangeAppointmentDate

    [Fact]
    public void ChangeAppointmentDate_WithValidFutureDate_ShouldUpdateDate()
    {
        // Arrange
        var appointment = CreateValidAppointment();
        var newDate = DateTime.UtcNow.AddDays(2);

        // Act
        appointment.ChangeAppointmentDate(newDate);

        // Assert
        Assert.Equal(newDate, appointment.AppointmentDate);
    }

    [Fact]
    public void ChangeAppointmentDate_WithPastDate_ShouldThrowArgumentException()
    {
        // Arrange
        var appointment = CreateValidAppointment();
        var pastDate = DateTime.UtcNow.AddMinutes(-1);

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => appointment.ChangeAppointmentDate(pastDate));
    }

    [Fact]
    public void ChangeAppointmentDate_WhenAppointmentIsCancelled_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Cancel();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => appointment.ChangeAppointmentDate(
                DateTime.UtcNow.AddDays(2)));
    }

    #endregion
}
