using Domain.Repositories;

namespace Domain.Doctors;

public interface IDoctorRepository : IGenericRepository<Doctor>
{
    Task<Doctor?> GetActiveByIdWithDepartmentAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Doctor>> GetWithDepartmentsAsync(bool includeInactive, CancellationToken cancellationToken = default);
    Task<Doctor?> GetDoctorWithSchedulesAndAppointmentsAsync(int id, CancellationToken cancellationToken = default);
    Task<Doctor?> GetBookingAvailabilityAsync(int id, CancellationToken cancellationToken = default);
}
