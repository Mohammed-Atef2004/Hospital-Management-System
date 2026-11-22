using Domain.Departments;
using Domain.Doctors;
using Domain.Entities;
using Domain.Medicines;
using Domain.Nurses;
using Domain.personnel;
using Domain.Shifts;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infastructure.Presistence.Database
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        private readonly IDataProtectionProvider? _dataProtectionProvider;
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<StockAdjustment> StockAdjustments { get; set; }
        public DbSet<Dispense> Dispenses { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<User> DomainUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IDataProtectionProvider? dataProtectionProvider = null)
            : base(options)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Staff>().ToTable("Staff");

            modelBuilder.Entity<Doctor>().ToTable("Doctors");

            modelBuilder.Entity<Nurse>().ToTable("Nurses");
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            if (_dataProtectionProvider is not null)
            {
                var protector = _dataProtectionProvider.CreateProtector("HospitalManagement.UserTwoFactorSecret.v1");
                modelBuilder.Entity<User>()
                    .Property(user => user.TwoFactorSecret)
                    .HasConversion(new ValueConverter<string?, string?>(
                        value => value == null ? null : protector.Protect(value),
                        value => value == null ? null : protector.Unprotect(value)));
                modelBuilder.Entity<User>()
                    .Property(user => user.PendingTwoFactorSecret)
                    .HasConversion(new ValueConverter<string?, string?>(
                        value => value == null ? null : protector.Protect(value),
                        value => value == null ? null : protector.Unprotect(value)));
            }
        }
    }
}
