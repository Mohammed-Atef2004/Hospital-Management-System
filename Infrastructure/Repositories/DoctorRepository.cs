
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

        public async Task<Doctor?> GetDoctorWithSchedulesAndAppointmentsAsync(int id)
        {
            return await _context.Doctors
                .Include(d => d.Shifts)
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
