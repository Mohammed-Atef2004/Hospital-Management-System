using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Departments
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task<bool> ExistsByNameAsync(string name, int? excludingDepartmentId = null, CancellationToken cancellationToken = default);
    }

}
