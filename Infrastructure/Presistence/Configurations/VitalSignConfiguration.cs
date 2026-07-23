using Domain.Entities;
using Domain.Nurses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Presistence.Configurations
{
    public class VitalSignConfiguration : IEntityTypeConfiguration<VitalSign>
    {
        public void Configure(EntityTypeBuilder<VitalSign> builder)
        {
            builder.ToTable("VitalSigns");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Temperature)
                   .HasColumnType("decimal(4,1)")
                   .IsRequired();
            builder.HasOne<Patient>()
                   .WithMany(p => p.VitalSigns)
                   .HasForeignKey(v => v.PatientId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne<Nurse>()
                   .WithMany()
                   .HasForeignKey(v => v.RecordedByNurseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(v => new { v.PatientId, v.RecordedAt });
        }
    }
}
