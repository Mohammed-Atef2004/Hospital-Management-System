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
    public class MedicalRecordConfiguration:IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            //MedicalRecord relationships
            builder.ToTable("MedicalRecords");

            builder.HasKey(mr => mr.Id);

            builder
                .HasOne(mr => mr.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(mr => mr.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(mr => mr.Prescriptions)
                .WithOne(pr => pr.MedicalRecord)
                .HasForeignKey(pr => pr.MedicalRecordId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(mr => mr.Doctor)
                .WithMany()
                .HasForeignKey(mr => mr.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
