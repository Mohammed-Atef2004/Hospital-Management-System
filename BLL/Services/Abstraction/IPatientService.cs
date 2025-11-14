using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Abstraction
{
    public interface IPatientService
    {
       public  Task<(bool, string)> AddPatientAsync(Patient patient);
       public Task<IEnumerable<Patient>> GetAllPatients(string? includeword = null);
       public Task<Patient?> GetPatientByIdAsync(int Id, string? includeword = null);
       public Task<(bool, string)> UpdatePatientAsync(Patient patient);
       public Task<(bool, string)> DeletePatientAsync(int Id);
       public Task<(bool, string)> AddAppointmentToPatient(Appointment appointment, int Id);
       public Task<IEnumerable<Appointment>> GetPatientAppointments(int Id,string? includeword=null);

    }
}
