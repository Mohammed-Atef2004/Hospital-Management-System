
using Domain.Departments;
using Domain.Doctors;
using Domain.Medicines;
using Domain.Nurses;
using Domain.PatientAssignments;
using Domain.personnel;
using Domain.Repositories;
using Domain.Schedules;
using Domain.Users;
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
        public IMedicineRepository Medicine { get; private set; }
        public INurseRepository Nurse { get; private set; }
        public IShiftRepository Shift { get; private set; }
        public IStaffRepository Staff { get; private set; }
        public IAssignPatientRepository PatientAssignment
        { get; private set; }
        public IUserRepository User { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Patient = new PatientRepository(_context);
            Department = new DepartmentRepository(_context);
            Doctor = new DoctorRepository(_context);

            Nurse = new NurseRepository(_context);
            Shift = new ShiftRepository(_context);
            Medicine = new MedicineRepository(_context);
            Staff = new StaffRepository(_context);
            PatientAssignment = new AssignPatientRepository(_context);
            User = new UserRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result;

        }

        public void Dispose()
        {
            _context.Dispose();
        }


    }
}
