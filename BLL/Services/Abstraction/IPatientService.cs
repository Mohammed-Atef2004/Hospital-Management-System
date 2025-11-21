using BLL.DTOs;
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
       public  Task<(bool, string)> AddPatientAsync(CreatePatientDTO patient);
       public Task<IEnumerable<PatientDTO>> GetAllPatients(string? includeword = null);
       public Task<PatientDTO?> GetPatientByIdAsync(int Id, string? includeword = null);
       public Task<(bool, string)> UpdatePatientAsync(CreatePatientDTO patient,int Id);
       public Task<(bool, string)> DeletePatientAsync(int Id);
       public Task<(bool, string)> AddAppointmentToPatient(Appointment appointment, int Id);
       public Task<IEnumerable<Appointment>> GetPatientAppointments(int Id,string? includeword=null);

    }
}
