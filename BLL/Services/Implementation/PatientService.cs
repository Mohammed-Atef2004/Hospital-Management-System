using AutoMapper;
using BLL.DTOs;
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
        private readonly IMapper _mapper;
        
        public PatientService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<(bool, string)> AddPatientAsync(CreatePatientDTO patient)
        {
            if(patient == null)
            {
                return Task.FromResult((false, "Patient is null, creating process failed"));
            }
            var result=_mapper.Map<Patient>(patient);
            _unitOfWork.Patient.Add(result);
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

        public async Task<IEnumerable<PatientDTO>> GetAllPatients(string? includeword)
        {
            var result = _unitOfWork.Patient.GetAll();
            var mappedresult=_mapper.Map<IEnumerable<PatientDTO>>(result);
            return await Task.FromResult(mappedresult);
        }

        public async Task<PatientDTO?> GetPatientByIdAsync(int Id, string? includeword)
        {
            var result = _unitOfWork.Patient.GetFirstOrDefault(x => x.Id == Id);
            var mappedresult = _mapper.Map<PatientDTO>(result);
            return await Task.FromResult(mappedresult);

        }

        public async Task<(bool, string)> UpdatePatientAsync(CreatePatientDTO patient, int Id)
        {
            var oldPatient = _unitOfWork.Patient.GetFirstOrDefault(x => x.Id == Id);
            var result=_mapper.Map<Patient>(patient);
            if (oldPatient != null)
            {
                oldPatient.FirstName= result.FirstName;
                oldPatient.LastName= result.LastName;
                oldPatient.Address= result.Address;
                _unitOfWork.Patient.Update(oldPatient);
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
