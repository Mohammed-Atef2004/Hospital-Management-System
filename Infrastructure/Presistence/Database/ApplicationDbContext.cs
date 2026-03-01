using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Presistence.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Staff> Staff { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Staff>().ToTable("Staff");

            modelBuilder.Entity<Doctor>().ToTable("Doctors");

            modelBuilder.Entity<Nurse>().ToTable("Nurses");
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
