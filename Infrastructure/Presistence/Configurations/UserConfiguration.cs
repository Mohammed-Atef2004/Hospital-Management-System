using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infastructure.Presistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("DomainUsers");
        builder.HasKey(user => user.Id);
        builder.HasIndex(user => user.IdentityId).IsUnique();
        builder.Property(user => user.IdentityId).IsRequired().HasMaxLength(450);
        builder.Property(user => user.Role).IsRequired();
        builder.Property(user => user.PasswordHistoryJson).IsRequired().HasMaxLength(4000);
        builder.Property(user => user.TwoFactorSecret).HasMaxLength(1024);
        builder.Property(user => user.DeleteReason).HasMaxLength(500);
        builder.Property(user => user.IsActive).IsRequired();
        builder.Property(user => user.FailedLoginAttempts).IsRequired();
        builder.Ignore(user => user.DomainEvents);
        builder.Ignore(user => user.PasswordHashes);

        builder.OwnsOne(user => user.Email, email =>
        {
            email.Property(value => value.Value).HasColumnName("Email").IsRequired().HasMaxLength(254);
            email.HasIndex(value => value.Value).IsUnique().HasFilter("[IsActive] = 1 AND [IsDeleted] = 0");
        });
        builder.Navigation(user => user.Email).IsRequired();

        builder.OwnsOne(user => user.Username, username =>
        {
            username.Property(value => value.Value).HasColumnName("Username").IsRequired().HasMaxLength(30);
            username.HasIndex(value => value.Value).IsUnique().HasFilter("[IsActive] = 1 AND [IsDeleted] = 0");
        });
        builder.Navigation(user => user.Username).IsRequired();

        builder.OwnsOne(user => user.FullName, name =>
        {
            name.Property(value => value.FirstName).HasColumnName("FirstName").IsRequired().HasMaxLength(100);
            name.Property(value => value.LastName).HasColumnName("LastName").IsRequired().HasMaxLength(100);
        });
        builder.Navigation(user => user.FullName).IsRequired();

        builder.OwnsOne(user => user.PhoneNumber, phone =>
        {
            phone.Property(value => value.Value).HasColumnName("PhoneNumber").HasMaxLength(16);
        });
    }
}
