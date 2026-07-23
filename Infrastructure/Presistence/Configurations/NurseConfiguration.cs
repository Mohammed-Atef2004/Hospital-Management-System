using Domain.Nurses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infastructure.Presistence.Configurations
{
    public class NurseConfiguration : IEntityTypeConfiguration<Nurse>
    {
        public void Configure(EntityTypeBuilder<Nurse> builder)
        {
            //Nurse relationships
            builder.ToTable("Nurses");


            //builder
            //   .HasOne(n => n.ApplicationUser)
            //   .WithOne(au => au.NurseProfile)
            //    .HasForeignKey<Nurse>(n => n.ApplicationUserId)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(n => n.Department)
                .WithMany(d => d.Nurses)
                .HasForeignKey(n => n.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Metadata
            .FindNavigation(nameof(Nurse.PatientAssignments))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
