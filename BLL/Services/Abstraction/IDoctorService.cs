using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Abstraction
{
    public interface IDoctorService
    {
       public  Task<(bool, string)> AddDoctorAsync(Doctor doctor);
       public Task<IEnumerable<Doctor>> GetAllDoctors(string? includeword = null);
       public Task<Doctor?> GetDoctorByIdAsync(int Id, string? includeword = null);
       public Task<(bool, string)> UpdateDoctorAsync(Doctor doctor);
       public Task<(bool, string)> DeleteDoctorAsync(int Id);
      

    }
}
