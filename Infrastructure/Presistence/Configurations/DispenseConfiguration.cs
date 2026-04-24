using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Presistence.Configurations
{
    public class DispenseConfiguration : IEntityTypeConfiguration<Dispense>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Dispense> builder)
        {
            builder.ToTable("Dispenses");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Quantity)
                .IsRequired();

            builder.Property(d => d.PharmacistId)
                .IsRequired();

            builder.Property(d => d.Date)
                .IsRequired();

            builder.HasOne(d => d.Medicine)
                .WithMany(m => m.Dispenses)
                .HasForeignKey(d => d.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Patient)
                .WithMany(p => p.Dispenses)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
