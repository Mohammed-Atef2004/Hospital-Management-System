using Domain.SharedKernel;
using FluentAssertions;

namespace Domain.Tests.Doctor;

public class DoctorScheduleTests
{
    [Fact]
    public void IsAvailableForAppointment_WhenInsideActiveShift_ShouldReturnTrue()
    {
        var doctor = CreateDoctor();
        var start = DateTime.UtcNow.AddDays(1);
        doctor.AddShift(start, start.AddHours(2));

        doctor.IsAvailableForAppointment(start.AddMinutes(30)).Should().BeTrue();
    }

    [Fact]
    public void IsAvailableForAppointment_AtShiftEnd_ShouldReturnFalse()
    {
        var doctor = CreateDoctor();
        var start = DateTime.UtcNow.AddDays(1);
        var end = start.AddHours(2);
        doctor.AddShift(start, end);

        doctor.IsAvailableForAppointment(end).Should().BeFalse();
    }

    [Fact]
    public void IsAvailableForAppointment_WhenShiftHasBeenRemoved_ShouldReturnFalse()
    {
        var doctor = CreateDoctor();
        var start = DateTime.UtcNow.AddDays(1);
        var shift = doctor.AddShift(start, start.AddHours(2));
        doctor.RemoveShift(shift.Id);

        doctor.IsAvailableForAppointment(start.AddMinutes(30)).Should().BeFalse();
    }

    [Fact]
    public void TransferToInactiveDepartment_ShouldThrowDomainException()
    {
        var doctor = CreateDoctor();
        var department = Domain.Departments.Department.Register("Cardiology", "Heart care");
        department.Deactivate();

        Action act = () => doctor.TransferToDepartment(department);

        act.Should().Throw<DomainException>();
    }

    private static Domain.Doctors.Doctor CreateDoctor()
        => Domain.Doctors.Doctor.Create("Ahmed", "Ali", "Addr", "City", "Spec");
}
