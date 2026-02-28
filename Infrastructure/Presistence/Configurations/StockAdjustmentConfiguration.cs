using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Presistence.Configurations
{
    internal class StockAdjustmentConfiguration : IEntityTypeConfiguration<StockAdjustment>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<StockAdjustment> builder)
        {
            builder.ToTable("StockAdjustments");

            builder.HasKey(sa => sa.Id);

            builder.Property(sa => sa.Quantity)
                .IsRequired();

            builder.Property(sa => sa.Reason)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(sa => sa.PharmacistId)
                .IsRequired();

            builder.Property(sa => sa.Date)
                .IsRequired();

            builder.HasOne(sa => sa.Medicine)
                .WithMany(m => m.StockAdjustments)
                .HasForeignKey(sa => sa.MedicineId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
