
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

        public Task<bool> ExistsByNameAsync(string name, int? excludingDepartmentId = null, CancellationToken cancellationToken = default)
        {
            var normalizedName = name.Trim().ToUpper();
            return _dbSet.AnyAsync(d => d.Name.ToUpper() == normalizedName && !d.IsDeleted &&
                (!excludingDepartmentId.HasValue || d.Id != excludingDepartmentId.Value), cancellationToken);
        }
    }

}
