using DAL.Database;
using DAL.Repositories.Abstraction;
using DAL.Repositories.Implementation;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IPatientRepository Patient { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Patient = new PatientRepository(_context);
            Department = new DepartmentRepository(_context);

        }

        public int Complete()
        {
            return _context.SaveChanges();

        }

        public void Dispose()
        {
            _context.Dispose();
        }


    }
}
