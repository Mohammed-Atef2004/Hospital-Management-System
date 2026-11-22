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
}
