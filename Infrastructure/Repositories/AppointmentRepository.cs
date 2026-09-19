using Domain.Entities;
using Domain.Interfaces;
using Infastructure.Presistence.Database;
using Infastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AppointmentRepository:GenericRepository<Appointment>,IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context) : base(context) { }
    }
}
