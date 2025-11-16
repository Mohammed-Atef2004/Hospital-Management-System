using DAL.Database;
using DAL.Models;
using DAL.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Stripe.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementation
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Doctor> _dbSet;
        public DoctorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Doctor>();
        }
        public void Update(Doctor doctor)
        {
            var existingDoctor = _dbSet.Find(doctor.Id);
            if (existingDoctor != null)
            {
                existingDoctor.ApplicationUserId = doctor.ApplicationUserId;
                existingDoctor.Specialization = doctor.Specialization;
                existingDoctor.DepartmentId = doctor.DepartmentId;
                existingDoctor.Appointments = doctor.Appointments;
                existingDoctor.Schedules = doctor.Schedules;
                existingDoctor.Name = doctor.Name;
                _dbSet.Update(existingDoctor);
                _context.SaveChanges();

            }
        }
    }
}
