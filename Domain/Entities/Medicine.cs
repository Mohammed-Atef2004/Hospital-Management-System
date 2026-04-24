
using Domain.Exceptions.Medicine;
using Domain.Repositories;
using System.Diagnostics;

namespace Domain.Entities
{
    public class Medicine : CommonData, IAggregateRoot
    {
        public int Id { get; private set; }
        public  string Name { get; private set; }
        public int Stock { get; private set; }
        public decimal Price { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        //public int PharmacyId { get; private set; }
        //public Pharmacy? Pharmacy { get; private set; }

        private ICollection<StockAdjustment> _stockAdjustments = new List<StockAdjustment>();
        public IReadOnlyCollection<StockAdjustment> StockAdjustments =>
            new List<StockAdjustment>(_stockAdjustments).AsReadOnly();

        private ICollection<Dispense> _dispenses = new List<Dispense>();
        public IReadOnlyCollection<Dispense> Dispenses =>
            new List<Dispense>(_dispenses).AsReadOnly();
        private Medicine() { }

        public Medicine(string name,decimal price,int quantity,DateTime expiredate, string? CreatedBy)
        {
        if(string.IsNullOrWhiteSpace(name))
                throw new InvalidMedicineNameException();
        if(price<=0)
                throw new InvalidMedicinePriceException();
        if(quantity<0)
                throw new InvalidMedicineQuantityException();
        if(expiredate <= DateTime.UtcNow)
                throw new InvalidExpiryDateException();
            Name = name;
            Price = price;
            Stock = quantity;
            ExpiryDate = expiredate;
        CreatedAt = DateTime.UtcNow;
            if (CreatedBy != null)
                this.CreatedBy = CreatedBy;
        }

        public void UpdateInfo(string? name, decimal? price, DateTime? expiryDate, string updatedBy)
        {
            if (name != null)
            {
                if (string.IsNullOrWhiteSpace(name)) throw new InvalidMedicineNameException();
                Name = name;
            }
            if (price.HasValue)
            {
                if (price.Value <= 0) throw new InvalidMedicinePriceException();
                Price = price.Value;
            }
            if (expiryDate.HasValue)
            {
                if (expiryDate.Value <= DateTime.UtcNow) throw new InvalidExpiryDateException();
                ExpiryDate = expiryDate.Value;
            }
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
        }

        //public void IncreaseStock(int amount) 
        //{
        //    if (amount <= 0)
        //        throw new InvalidMedicineQuntityException();
        //    Stock += amount;
        //}
        public bool IsExpired() 
        {
        if (ExpiryDate < DateTime.UtcNow)
                return true;
            return false;
        }
        public bool IsOutOfStock() 
        {
            if (Stock ==0)
                return true;
            return false;
        }
        //public void DecreaseStock(int amount)
        //{
        //    if (amount <= 0)
        //        throw new InvalidMedicineQuntityException();
        //    if (amount > Stock)
        //        throw new NotEnoughStockException();
        //    if(isExpired())
        //        throw new MedicineExpiredException();
        //    Stock -= amount;
        //}
        public void updatePrice(decimal newPrice) 
        {
            if (newPrice <= 0)
                throw new InvalidMedicinePriceException();
            Price = newPrice;
        }

        public void AdjustStock(int amount, string reason, int pharmacistId)
        {
            if (amount < 0 && Math.Abs(amount) > Stock)
                throw new NotEnoughStockException();

            _stockAdjustments.Add(new StockAdjustment(Id, amount, reason, pharmacistId));
            Stock += amount;

            UpdatedAt = DateTime.UtcNow;
            //UpdatedBy = pharmacist;
        }
        public void DispenseMedicine(int quantity, int patientId, int pharmacistId)
        {   
            if (quantity <= 0) 
                throw new InvalidMedicineQuantityException();
            if (IsExpired()) 
                throw new MedicineExpiredException();
            if (quantity > Stock)
                throw new NotEnoughStockException();

            _dispenses.Add(new Dispense(Id, patientId, quantity, pharmacistId));
            Stock -= quantity;

            UpdatedAt = DateTime.UtcNow;
            //UpdatedBy = dispensedBy;
        }
    }
}
