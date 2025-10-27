using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    public class PharmacyConfiguration:IEntityTypeConfiguration<Pharmacy>
    {
        public void Configure(EntityTypeBuilder<Pharmacy> builder)
        {
            //Pharmacy relationships
            builder.ToTable("Pharmacies");

            builder.HasKey(p => p.Id);

            builder
               .HasMany(p=> p.Medicines)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);
        }
    
    }
}
