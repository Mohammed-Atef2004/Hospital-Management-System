using DAL.Database;
using DAL.Models;
using DAL.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementation
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Patient> _dbSet;
        public PatientRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Patient>();
        }
        public void Update(Patient patient)
        {
            var existingPatient = _dbSet.Find(patient.Id);
            if (existingPatient != null)
            {
                existingPatient.NurseId = patient.NurseId;
                existingPatient.ApplicationUser=patient.ApplicationUser;
                existingPatient.Appointments= patient.Appointments;
                existingPatient.ApplicationUserId = patient.ApplicationUserId;
                _dbSet.Update(existingPatient);
                _context.SaveChanges();

            }
        }
    }
}
