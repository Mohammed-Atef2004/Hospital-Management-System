
using Domain.Departments;
using Infastructure.Presistence.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repositories
{

    public class DepartmentRepository
    : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context) { }

        public Task<bool> ExistsByNameAsync(string name)
            => _dbSet.AnyAsync(d => d.Name == name && !d.IsDeleted);
    }

}
