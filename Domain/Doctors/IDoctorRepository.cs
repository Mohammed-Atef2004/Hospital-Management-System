using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Doctors
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        public interface IDoctorRepository : IGenericRepository<Doctor>
        {
            Task<Doctor?> GetDoctorWithSchedulesAndAppointmentsAsync(int id);
        }
    }
}
