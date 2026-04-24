using Domain.Nurses;
using Infastructure.Presistence.Database;
using Infastructure.Repositories;

namespace Infrastructure.Repositories
{
    public class NurseRepository
       : GenericRepository<Nurse>, INurseRepository
    {
        public NurseRepository(ApplicationDbContext context) : base(context) { }
    }
}
