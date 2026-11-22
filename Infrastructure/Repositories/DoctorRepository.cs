
using Domain.Doctors;
using Infastructure.Presistence.Database;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly ApplicationDbContext _context;
        public DoctorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Doctor?> GetDoctorWithSchedulesAndAppointmentsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Doctors
                .AsNoTracking()
                .Include(d => d.Department)
                .Include(d => d.Shifts)
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        public async Task<Doctor?> GetBookingAvailabilityAsync(int id, CancellationToken cancellationToken = default)
            => await _context.Doctors
                .AsNoTracking()
                .Include(doctor => doctor.Shifts)
                .Include(doctor => doctor.Appointments.Where(appointment => !appointment.IsDeleted && !appointment.IsCancelled))
                .SingleOrDefaultAsync(doctor => doctor.Id == id && !doctor.IsDeleted, cancellationToken);

        public Task<Doctor?> GetActiveByIdWithDepartmentAsync(int id, CancellationToken cancellationToken = default)
            => _context.Doctors
                .AsNoTracking()
                .Include(doctor => doctor.Department)
                .SingleOrDefaultAsync(doctor => doctor.Id == id && !doctor.IsDeleted, cancellationToken);

        public async Task<IReadOnlyList<Doctor>> GetWithDepartmentsAsync(bool includeInactive, CancellationToken cancellationToken = default)
            => await _context.Doctors
                .AsNoTracking()
                .Where(doctor => includeInactive || !doctor.IsDeleted)
                .Include(doctor => doctor.Department)
                .OrderBy(doctor => doctor.LastName)
                .ThenBy(doctor => doctor.FirstName)
                .ToListAsync(cancellationToken);
    }
}
