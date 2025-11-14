using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Abstraction
{
    public interface IDepartmentService
    {
       public  Task<(bool, string)> AddDepartmentAsync(Department department);
       public Task<IEnumerable<Department>> GetAllDepartments(string? includeword = null);
       public Task<Department?> GetDepartmentByIdAsync(int Id, string? includeword = null);
       public Task<(bool, string)> UpdateDepartmentAsync(Department department);
       public Task<(bool, string)> DeleteDepartmentAsync(int Id);
      

    }
}
