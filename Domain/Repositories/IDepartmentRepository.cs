using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        public void Update(Department department);
    }
}
