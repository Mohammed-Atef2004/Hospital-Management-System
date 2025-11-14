using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Abstraction
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        public void Update(Department department);
    }
}
