using Domain.PatientAssignments;
using Domain.Shifts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Presistence.Configurations
{
    public class PatientAssignmentConfiguration : IEntityTypeConfiguration<PatientAssignment>
    {
        public void Configure(EntityTypeBuilder<PatientAssignment> builder)
        {
            builder.ToTable("PatientAssignments");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(a => a.AssignedAt)
                   .IsRequired();

            builder.Property(a => a.IsActive)
              .HasDefaultValue(true)
              .IsRequired();
            builder.HasOne(a => a.Patient)
               .WithMany()
               .HasForeignKey(a => a.PatientId)
               .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.Nurse)
                   .WithMany(n => n.PatientAssignments)
                   .HasForeignKey(a => a.NurseId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Shift>()
             .WithMany().HasForeignKey(a => a.ShiftId).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => new { a.ShiftId, a.NurseId, a.IsActive })
                   .HasDatabaseName("IX_PatientAssignments_Shift_Nurse_Active");
        }
    }
}
