
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patient { get; }
        IDepartmentRepository Department { get; }
		IDoctorRepository Doctor { get; }
        IMedicineRepository Medicine { get; }
		Task<int> CompleteAsync();
    }
}
