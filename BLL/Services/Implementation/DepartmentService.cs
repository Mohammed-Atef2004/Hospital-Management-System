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
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<(bool, string)> AddDepartmentAsync(Department department)
        {
            if(department == null)
            {
                return Task.FromResult((false, "Department is null, creating process failed"));
            }
            _unitOfWork.Department.Add(department);
            _unitOfWork.Complete();
            return Task.FromResult((true, "Department created successfully"));
        }

        public async Task<(bool, string)> DeleteDepartmentAsync(int Id)
        {
            var result = _unitOfWork.Department.GetFirstOrDefault(x=>x.Id==Id);
            if(result == null)
            {
                return await Task.FromResult((false, "Department not found, deletion failed"));
            }
            _unitOfWork.Department.Remove(result);
            _unitOfWork.Complete();
            return await Task.FromResult((true, "Department deleted successfully"));
        }

        public async Task<IEnumerable<Department>> GetAllDepartments(string? includeword)
        {
            var result = _unitOfWork.Department.GetAll();
            return await Task.FromResult(result);
        }

        public async Task<Department?> GetDepartmentByIdAsync(int Id, string? includeword)
        {
            var result = _unitOfWork.Department.GetFirstOrDefault(x => x.Id == Id);
            return await Task.FromResult(result);

        }

        public async Task<(bool, string)> UpdateDepartmentAsync(Department Department)
        {
            var oldPatient = _unitOfWork.Department.GetFirstOrDefault(x => x.Id == Department.Id);
            if (oldPatient != null)
            {
                _unitOfWork.Department.Update(Department);
                _unitOfWork.Complete();
                return await Task.FromResult((true, "Department Updated Successfully"));
            }
            return await Task.FromResult((false, "Department not found, update failed"));
        }
        

    }
}
