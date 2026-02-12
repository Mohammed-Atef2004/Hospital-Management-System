
using Domain.Entities;
using Domain.Repositories;
using Infastructure.Presistence.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Department> _dbSet;
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Department>();
        }

        public bool Add(Department entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Department> GetAll(System.Linq.Expressions.Expression<Func<Department, bool>>? predict = null, string? includeword = null)
        {
            throw new NotImplementedException();
        }

        public Department GetFirstOrDefault(System.Linq.Expressions.Expression<Func<Department, bool>>? predict = null, string? includeword = null)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Department entity)
        {
            throw new NotImplementedException();
        }

        public bool RemoveRange(IEnumerable<Department> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Department department)
        {
            var existingDepartment = _dbSet.Find(department.Id);
            if (existingDepartment != null)
            {
                existingDepartment.Name = department.Name;
                existingDepartment.Description = department.Description;
                existingDepartment.Doctors = department.Doctors;
                existingDepartment.Nurses = department.Nurses;
                _dbSet.Update(existingDepartment);
                _context.SaveChanges();

            }
        }

       
    }
}
