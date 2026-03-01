using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Presistence.Configurations
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable("Schedules");

            builder.HasKey(s => s.Id);

            builder
                 .Property(s => s.DayOfWeek)
                 .IsRequired();

            builder.Property(s => s.StartTime)
                .IsRequired();

            builder.Property(s => s.EndTime)
                .IsRequired();

            builder.HasIndex(s => new { s.DayOfWeek, s.StartTime, s.EndTime }).IsUnique();
        }
    }
}
