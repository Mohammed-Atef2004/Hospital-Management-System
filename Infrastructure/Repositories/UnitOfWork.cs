
using Domain.Repositories;
using Infastructure.Presistence.Database;
using Infrastructure.Repositories;

namespace Infastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IPatientRepository Patient { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public IDoctorRepository Doctor { get; private set; }
        public INurseRepository Nurse { get; private set; }
        public IScheduleRepository Schedule { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Patient = new PatientRepository(_context);
            Department = new DepartmentRepository(_context);
            Doctor = new DoctorRepository(_context);
            Nurse = new NurseRepository(_context);
            Schedule = new ScheduleRepository(_context);


        }

        public async Task<int> CompleteAsync()
        {
            return _context.SaveChanges();

        }

        public void Dispose()
        {
            _context.Dispose();
        }


    }
}
