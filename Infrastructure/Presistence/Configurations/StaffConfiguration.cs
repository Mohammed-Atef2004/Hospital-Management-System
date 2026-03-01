using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Presistence.Configurations
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasMany(s => s.Schedules)
               .WithOne(s => s.Staff)
               .HasForeignKey(s => s.StaffId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(s => s.Schedules)
                   .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
