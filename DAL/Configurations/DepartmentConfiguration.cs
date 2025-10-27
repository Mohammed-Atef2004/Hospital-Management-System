using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    public class DepartmentConfiguration:IEntityTypeConfiguration<Department>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.Description)
                .HasMaxLength(500);

            builder.HasMany(d => d.Doctors)
               .WithOne(doc => doc.Department)
               .HasForeignKey(doc => doc.DepartmentId)
               .OnDelete(DeleteBehavior.Restrict);  

            builder.HasMany(d => d.Nurses)
                .WithOne(n => n.Department)
                .HasForeignKey(n => n.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
