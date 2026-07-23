
using Domain.Entities;
using Domain.Repositories;
using Infastructure.Presistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Repositories
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
                existingPatient.Appointments = patient.Appointments;
                existingPatient.MedicalRecords = patient.MedicalRecords;
                existingPatient.Prescriptions = patient.Prescriptions;
                existingPatient.FirstName = patient.FirstName;
                existingPatient.LastName = patient.LastName;
                _dbSet.Update(existingPatient);
                _context.SaveChanges();

            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {

            return await _context.Set<Patient>()
                .AnyAsync(p => p.Id == id && !p.IsDeleted, cancellationToken);
        }
    }
}
