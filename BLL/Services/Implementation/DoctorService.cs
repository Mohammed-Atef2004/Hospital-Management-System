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
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DoctorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<(bool, string)> AddDoctorAsync(Doctor doctor)
        {
            if(doctor == null)
            {
                return Task.FromResult((false, "Doctor is null, creating process failed"));
            }
            _unitOfWork.Doctor.Add(doctor);
            _unitOfWork.Complete();
            return Task.FromResult((true, "Doctor created successfully"));
        }

        public async Task<(bool, string)> DeleteDoctorAsync(int Id)
        {
            var result = _unitOfWork.Doctor.GetFirstOrDefault(x=>x.Id==Id);
            if(result == null)
            {
                return await Task.FromResult((false, "Doctor not found, deletion failed"));
            }
            _unitOfWork.Doctor.Remove(result);
            _unitOfWork.Complete();
            return await Task.FromResult((true, "Doctor deleted successfully"));
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctors(string? includeword)
        {
            var result = _unitOfWork.Doctor.GetAll();
            return await Task.FromResult(result);
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int Id, string? includeword)
        {
            var result = _unitOfWork.Doctor.GetFirstOrDefault(x => x.Id == Id);
            return await Task.FromResult(result);

        }

        public async Task<(bool, string)> UpdateDoctorAsync(Doctor Doctor)
        {
            var oldPatient = _unitOfWork.Doctor.GetFirstOrDefault(x => x.Id == Doctor.Id);
            if (oldPatient != null)
            {
                _unitOfWork.Doctor.Update(Doctor);
                _unitOfWork.Complete();
                return await Task.FromResult((true, "Doctor Updated Successfully"));
            }
            return await Task.FromResult((false, "Doctor not found, update failed"));
        }
        

    }
}
