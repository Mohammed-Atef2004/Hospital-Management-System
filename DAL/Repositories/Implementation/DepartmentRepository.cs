using DAL.Database;
using DAL.Models;
using DAL.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Stripe.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementation
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
