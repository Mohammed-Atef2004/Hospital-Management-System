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
    public class NurseConfiguration:IEntityTypeConfiguration<Nurse>
    {
        public void Configure(EntityTypeBuilder<Nurse> builder)
        {
            //Nurse relationships
            builder.ToTable("Nurses");

            builder.HasKey(n => n.Id);

            builder
               .HasOne(n => n.ApplicationUser)
               .WithOne(au => au.NurseProfile)
                .HasForeignKey<Nurse>(n => n.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(n => n.Department)
                .WithMany(d => d.Nurses)
                .HasForeignKey(n => n.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
