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
    public class PrescriptionConfiguration:IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            //Prescription relationships
            builder.ToTable("Prescriptions");

            builder.HasKey(pr => pr.Id);

            builder
               .HasOne(pr => pr.Patient)
               .WithMany(p => p.Prescriptions)
               .HasForeignKey(pr => pr.PatientId)
               .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(pr => pr.MedicalRecord)
                .WithMany(mr => mr.Prescriptions)
                .HasForeignKey(pr => pr.MedicalRecordId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    
    }
}
