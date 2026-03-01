namespace Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patient { get; }
        IDepartmentRepository Department { get; }
        IDoctorRepository Doctor { get; }
        INurseRepository Nurse { get; }
        IScheduleRepository Schedule { get; }
        Task<int> CompleteAsync();
    }
}
