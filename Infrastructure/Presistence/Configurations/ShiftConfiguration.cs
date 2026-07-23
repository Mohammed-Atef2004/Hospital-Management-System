using Domain.Shifts;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Presistence.Configurations
{
    public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Shift> builder)
        {
            builder.ToTable("Shifts");

            builder.HasKey(s => s.Id);

            builder
                 .Property(s => s.StaffId)
                 .IsRequired();

            builder.Property(s => s.StartDateTime)
                .IsRequired();

            builder.Property(s => s.EndDateTime)
                .IsRequired();

            builder.HasIndex(s => new { s.StaffId, s.StartDateTime, s.EndDateTime }).IsUnique();
        }
    }
}
