using Domain.Entities;
using Domain.Repositories;
using Infastructure.Presistence.Database;
using Infastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MedicineRepository : GenericRepository<Medicine>, IMedicineRepository
    {
        public MedicineRepository(ApplicationDbContext context) : base(context) { }
        public async Task<Medicine?> GetMedicineWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(m => m.StockAdjustments)
                .Include(m => m.Dispenses)
                    .ThenInclude(d => d.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<IEnumerable<Medicine>> GetLowStockMedicinesAsync(int threshold)
        {
            return await _dbSet
                .Where(m => m.Stock <= threshold)
                .ToListAsync();
        }
        public async Task<IEnumerable<Medicine>> GetExpiredMedicinesAsync()
        {
            var today = DateTime.UtcNow;

            return await _dbSet
                .Where(m => m.ExpiryDate < today)
                .ToListAsync();
        }
        public async Task<IEnumerable<Medicine>> GetMedicinesExpiringInNextMonthAsync()
        {
            var today = DateTime.UtcNow.Date;
            var oneMonthLater = today.AddMonths(1);

            return await _dbSet
                .Where(m => m.ExpiryDate >= today && m.ExpiryDate <= oneMonthLater)
                .ToListAsync();
        }
        public Task<bool> ExistsByNameAsync(string name)
            => _dbSet.AnyAsync(d => d.Name == name && !d.IsDeleted);
    }
}
