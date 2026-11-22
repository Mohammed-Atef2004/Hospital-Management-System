using Domain.SharedKernel;
using PatientEntity = Domain.Entities.Patient;

namespace Domain.Tests.Patient;

public sealed class PatientProfileTests
{
    [Fact]
    public void RegisterProfile_ShouldRequireAccountAndNormalizeProfileFields()
    {
        var userId = Guid.NewGuid();

        var patient = PatientEntity.RegisterProfile(userId, "  Amal ", " Nabil ", "  15 Main St  ", " Cairo ", " 12345 ");

        Assert.Equal(userId, patient.UserId);
        Assert.Equal("Amal", patient.FirstName);
        Assert.Equal("Nabil", patient.LastName);
        Assert.Equal("15 Main St", patient.Address);
        Assert.Equal("Cairo", patient.City);
        Assert.Equal("12345", patient.PhoneNumber);
    }

    [Fact]
    public void RegisterProfile_ShouldRejectMissingAccount()
    {
        Assert.Throws<DomainException>(() =>
            PatientEntity.RegisterProfile(Guid.Empty, "Amal", "Nabil", "15 Main St", null, null));
    }

    [Fact]
    public void UpdateProfile_ShouldEnforceStorageLimits()
    {
        var patient = PatientEntity.RegisterProfile(Guid.NewGuid(), "Amal", "Nabil", "15 Main St", null, null);

        Assert.Throws<DomainException>(() =>
            patient.UpdateProfile("A", "B", new string('x', 251), null, null));
    }
}
