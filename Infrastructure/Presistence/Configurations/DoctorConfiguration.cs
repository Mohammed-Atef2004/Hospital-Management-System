using Domain.Doctors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctors");



        // ضبط خصائص النصوص
        builder.Property(d => d.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(d => d.LastName).IsRequired().HasMaxLength(50);
        builder.Property(d => d.Specialization).IsRequired().HasMaxLength(100);
        builder.Property(d => d.PhoneNumber).HasMaxLength(20);
        builder.Property(d => d.Address).HasMaxLength(250);
        builder.Property(d => d.City).HasMaxLength(50);

        // علاقة 1:1 مع ApplicationUser
        //builder
        //   .HasOne(d => d.ApplicationUser)
        //   .WithOne(u => u.DoctorProfile) // تأكد أن الاسم في ApplicationUser هو DoctorProfile
        //   .HasForeignKey<Doctor>(d => d.ApplicationUserId)
        //   .OnDelete(DeleteBehavior.Restrict);

        // علاقة 1:N مع المواعيد
        builder
            .HasMany(d => d.Appointments)
            .WithOne(a => a.Doctor)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        // علاقة N:1 مع الأقسام
        builder
            .HasOne(d => d.Department)
            .WithMany(dep => dep.Doctors)
            .HasForeignKey(d => d.DepartmentId)
            .OnDelete(DeleteBehavior.SetNull); // ملاحظة: لو القسم اتمسح، الدكاترة يفضلوا موجودين بس بدون قسم


    }
}