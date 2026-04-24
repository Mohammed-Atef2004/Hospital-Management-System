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
