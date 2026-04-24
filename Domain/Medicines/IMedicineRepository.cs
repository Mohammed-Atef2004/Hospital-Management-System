using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Medicines
{
    public interface IMedicineRepository : IGenericRepository<Medicine>
    {
        Task<IEnumerable<Medicine>> GetMedicinesExpiringInNextMonthAsync();
        Task<IEnumerable<Medicine>> GetLowStockMedicinesAsync(int threshold);
        Task<Medicine?> GetMedicineWithDetailsAsync(int id);
        Task<IEnumerable<Medicine>> GetExpiredMedicinesAsync();
        Task<bool> ExistsByNameAsync(string name);
    }
}
