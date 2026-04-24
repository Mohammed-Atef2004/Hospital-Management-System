using Domain.Departments;
using Domain.Doctors;
using Domain.Medicines;
using Domain.Nurses;
using Domain.Schedules;

namespace Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patient { get; }
        IDepartmentRepository Department { get; }
		IDoctorRepository Doctor { get; }
        IMedicineRepository Medicine { get; }
        IScheduleRepository Schedule { get; }
        INurseRepository Nurse { get; }

        Task<int> CompleteAsync();
    }
}
