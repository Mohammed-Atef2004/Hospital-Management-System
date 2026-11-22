using Domain.Users;
using Domain.Medicines;
using Infastructure.Presistence.Database;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace Domain.Tests.Users;

public sealed class AuthenticationPersistenceTests
{
    [Fact]
    public void UserModel_ShouldEncryptTwoFactorSecretsAtRest()
    {
        IDataProtectionProvider dataProtectionProvider = new EphemeralDataProtectionProvider();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=localhost;Database=HospitalManagementSystemTests;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;
        using var context = new ApplicationDbContext(
            options,
            dataProtectionProvider);

        var property = context.Model.FindEntityType(typeof(User))!
            .FindProperty(nameof(User.TwoFactorSecret))!;
        var converter = property.GetValueConverter();
        const string secret = "JBSWY3DPEHPK3PXP";

        var encrypted = converter!.ConvertToProvider(secret);
        Assert.IsType<string>(encrypted);
        Assert.NotEqual(secret, encrypted);
        Assert.Equal(secret, converter.ConvertFromProvider(encrypted));

        var pendingProperty = context.Model.FindEntityType(typeof(User))!
            .FindProperty(nameof(User.PendingTwoFactorSecret))!;
        var pendingConverter = pendingProperty.GetValueConverter();
        var encryptedPending = pendingConverter!.ConvertToProvider(secret);
        Assert.NotEqual(secret, encryptedPending);
        Assert.Equal(secret, pendingConverter.ConvertFromProvider(encryptedPending));
    }

    [Fact]
    public void MedicineStockVersion_ShouldBeConfiguredAsOptimisticConcurrencyToken()
    {
        IDataProtectionProvider dataProtectionProvider = new EphemeralDataProtectionProvider();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=localhost;Database=HospitalManagementSystemTests;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;
        using var context = new ApplicationDbContext(options, dataProtectionProvider);

        var property = context.Model.FindEntityType(typeof(Medicine))!
            .FindProperty(nameof(Medicine.RowVersion))!;

        Assert.True(property.IsConcurrencyToken);
        Assert.Equal("rowversion", property.GetColumnType());
    }

    [Fact]
    public void UserStaffProfile_ShouldHaveUniqueOptionalForeignKey()
    {
        IDataProtectionProvider dataProtectionProvider = new EphemeralDataProtectionProvider();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=localhost;Database=HospitalManagementSystemTests;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;
        using var context = new ApplicationDbContext(options, dataProtectionProvider);

        var userEntity = context.Model.FindEntityType(typeof(User))!;
        var staffIdProperty = userEntity.FindProperty(nameof(User.StaffId))!;
        var staffRelationship = userEntity.GetForeignKeys()
            .Single(key => key.Properties.Contains(staffIdProperty));
        var staffIndex = userEntity.GetIndexes()
            .Single(index => index.Properties.Contains(staffIdProperty));

        Assert.True(staffRelationship.IsUnique);
        Assert.True(staffIdProperty.IsNullable);
        Assert.True(staffIndex.IsUnique);
    }

    [Fact]
    public void PatientAccountLink_ShouldHaveOptionalUniqueForeignKey()
    {
        IDataProtectionProvider dataProtectionProvider = new EphemeralDataProtectionProvider();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=localhost;Database=HospitalManagementSystemTests;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;
        using var context = new ApplicationDbContext(options, dataProtectionProvider);

        var patientEntity = context.Model.FindEntityType(typeof(Domain.Entities.Patient))!;
        var userIdProperty = patientEntity.FindProperty(nameof(Domain.Entities.Patient.UserId))!;
        var relationship = patientEntity.GetForeignKeys()
            .Single(key => key.Properties.Contains(userIdProperty));
        var index = patientEntity.GetIndexes().Single(item => item.Properties.Contains(userIdProperty));

        Assert.True(relationship.IsUnique);
        Assert.True(userIdProperty.IsNullable);
        Assert.True(index.IsUnique);
    }
}
