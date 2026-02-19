using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Presistence.Configurations
{
    public class ScheduleConfiguration: IEntityTypeConfiguration<Schedule>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable("Schedules");

            builder.HasKey(s => s.Id);

            builder
                 .Property(s => s.DayofWeek)
                 .IsRequired();

            builder.Property(s => s.StartTime)
                .IsRequired();

            builder.Property(s => s.EndTime)
                .IsRequired();

            builder
                .HasOne(s => s.Doctor)
                .WithMany(d => d.Schedules)
                .HasForeignKey(s => s.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
