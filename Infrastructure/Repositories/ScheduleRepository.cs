using Domain.Entities;
using Domain.Repositories;
using Infastructure.Presistence.Database;
using Infastructure.Repositories;

namespace Infrastructure.Repositories
{
    public class ScheduleRepository
           : GenericRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(ApplicationDbContext context) : base(context) { }

    }
}
