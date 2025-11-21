using BLL.DTOs;
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
       public  Task<(bool, string)> AddDoctorAsync(CreateDoctorDTO doctor);
       public Task<IEnumerable<DoctorDTO>> GetAllDoctors(string? includeword = null);
       public Task<DoctorDTO?> GetDoctorByIdAsync(int Id, string? includeword = null);
       public Task<(bool, string)> UpdateDoctorAsync(CreateDoctorDTO doctor,int Id);
       public Task<(bool, string)> DeleteDoctorAsync(int Id);
       

    }
}
