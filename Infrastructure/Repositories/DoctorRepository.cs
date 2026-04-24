
using Domain.Doctors;
using Infastructure.Presistence.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .Include(d => d.Schedules)
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
