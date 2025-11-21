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
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public DoctorService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<(bool, string)> AddDoctorAsync(CreateDoctorDTO doctor)
        {
            if(doctor == null)
            {
                return Task.FromResult((false, "Doctor is null, creating process failed"));
            }
            var result=_mapper.Map<Doctor>(doctor);
            _unitOfWork.Doctor.Add(result);
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

        public async Task<IEnumerable<DoctorDTO>> GetAllDoctors(string? includeword)
        {
            var result = _unitOfWork.Doctor.GetAll();
            var mappedresult=_mapper.Map<IEnumerable<DoctorDTO>>(result);
            return await Task.FromResult(mappedresult);
        }

        public async Task<DoctorDTO?> GetDoctorByIdAsync(int Id, string? includeword)
        {
            var result = _unitOfWork.Doctor.GetFirstOrDefault(x => x.Id == Id);
            var mappedresult = _mapper.Map<DoctorDTO>(result);
            return await Task.FromResult(mappedresult);

        }

        public async Task<(bool, string)> UpdateDoctorAsync(CreateDoctorDTO doctor, int Id)
        {
            var oldDoctor = _unitOfWork.Doctor.GetFirstOrDefault(x => x.Id == Id);
            var result=_mapper.Map<Doctor>(doctor);
            if (oldDoctor != null)
            {
                oldDoctor.FirstName= result.FirstName;
                oldDoctor.LastName= result.LastName;
                oldDoctor.Address= result.Address;
                _unitOfWork.Doctor.Update(oldDoctor);
                _unitOfWork.Complete();
                return await Task.FromResult((true, "Doctor Updated Successfully"));
            }
            return await Task.FromResult((false, "Doctor not found, update failed"));
        }
       
    }
}
