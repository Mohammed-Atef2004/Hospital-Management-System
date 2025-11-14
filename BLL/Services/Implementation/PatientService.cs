using BLL.Services.Abstraction;
using DAL.Models;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<(bool, string)> AddPatientAsync(Patient patient)
        {
            if(patient == null)
            {
                return Task.FromResult((false, "Patient is null, creating process failed"));
            }
            _unitOfWork.Patient.Add(patient);
            _unitOfWork.Complete();
            return Task.FromResult((true, "Patient created successfully"));
        }

        public async Task<(bool, string)> DeletePatientAsync(int Id)
        {
            var result = _unitOfWork.Patient.GetFirstOrDefault(x=>x.Id==Id);
            if(result == null)
            {
                return await Task.FromResult((false, "Patient not found, deletion failed"));
            }
            _unitOfWork.Patient.Remove(result);
            _unitOfWork.Complete();
            return await Task.FromResult((true, "Patient deleted successfully"));
        }

        public async Task<IEnumerable<Patient>> GetAllPatients(string? includeword)
        {
            var result = _unitOfWork.Patient.GetAll();
            return await Task.FromResult(result);
        }

        public async Task<Patient?> GetPatientByIdAsync(int Id, string? includeword)
        {
            var result = _unitOfWork.Patient.GetFirstOrDefault(x => x.Id == Id);
            return await Task.FromResult(result);

        }

        public async Task<(bool, string)> UpdatePatientAsync(Patient patient)
        {
            var oldPatient = _unitOfWork.Patient.GetFirstOrDefault(x => x.Id == patient.Id);
            if (oldPatient != null)
            {
                _unitOfWork.Patient.Update(patient);
                _unitOfWork.Complete();
                return await Task.FromResult((true, "Patient Updated Successfully"));
            }
            return await Task.FromResult((false, "Patient not found, update failed"));
        }
        public async Task<(bool, string)> AddAppointmentToPatient(Appointment appointment,int Id)
        {
            var patient=_unitOfWork.Patient.GetFirstOrDefault(x=>x.Id==Id);
            if (patient != null)
            {
                patient.Appointments.Add(appointment);
                _unitOfWork.Patient.Update(patient);
                _unitOfWork.Complete();
                return await Task.FromResult((true, "Appointment Added Successfully"));
            }
            if (appointment == null)
            {
                return await Task.FromResult((false, "The Appointment is Invalid"));

            }
            return await Task.FromResult((false, "Can't found the user "));
        }
        public async Task<IEnumerable<Appointment>> GetPatientAppointments(int Id, string? includeword = null)
        {
            var patient = _unitOfWork.Patient.GetFirstOrDefault(x => x.Id == Id,includeword:"Appointments");
            var result = patient.Appointments;
            return await Task.FromResult(result);
           
        }

    }
}
