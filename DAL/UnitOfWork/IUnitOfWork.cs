using DAL.Repositories.Abstraction;
using DAL.Repositories.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patient { get; }
        IDepartmentRepository Department { get; }
		IDoctorRepository Doctor { get; }
		int Complete();
    }
}
