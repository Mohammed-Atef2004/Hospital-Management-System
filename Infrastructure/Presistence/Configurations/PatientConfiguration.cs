using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infastructure.Presistence.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            //Patient relationships
            builder.ToTable("Patients");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Address).IsRequired().HasMaxLength(250);
            builder.Property(p => p.City).HasMaxLength(100);
            builder.Property(p => p.PhoneNumber).HasMaxLength(20);
            builder.HasIndex(p => p.UserId).IsUnique().HasFilter("[UserId] IS NOT NULL AND [IsDeleted] = 0");
            builder.HasOne(p => p.Account)
                .WithOne()
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder
            //   .HasOne(p => p.ApplicationUser)
            //   .WithOne(au => au.PatientProfile)
            //    .HasForeignKey<Patient>(p => p.ApplicationUserId)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(p => p.MedicalRecords)
                .WithOne(mr => mr.Patient)
                .HasForeignKey(mr => mr.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(p => p.Prescriptions)
                .WithOne(pr => pr.Patient)
                .HasForeignKey(pr => pr.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.VitalSigns)
           .WithOne()
           .HasForeignKey(v => v.PatientId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.VitalSigns)
              .WithOne()
              .HasForeignKey(v => v.PatientId)
               .OnDelete(DeleteBehavior.Cascade);

            var navigation = builder.Metadata.FindNavigation(nameof(Patient.VitalSigns));
            navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        }

    }
}
