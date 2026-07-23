using Domain.Departments;
using Domain.Doctors;
using Domain.Medicines;
using Domain.Nurses;
using Domain.PatientAssignments;
using Domain.personnel;
using Domain.Schedules;


namespace Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patient { get; }
        IDepartmentRepository Department { get; }
        IDoctorRepository Doctor { get; }
        IMedicineRepository Medicine { get; }
        IShiftRepository Shift { get; }
        INurseRepository Nurse { get; }
        IStaffRepository Staff { get; }
        IAssignPatientRepository PatientAssignment
        { get; }

        Task<int> CompleteAsync();
    }
}
