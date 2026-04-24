using Domain.Medicines;
using Domain.Medicines.Exceptions;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class StockAdjustment
    {
        public int Id { get; private set; }
        public int Quantity { get; private set; }
        public string Reason { get; private set; }
        public int PharmacistId { get; private set; }
        public DateTime Date { get; private set; }

        public int MedicineId { get; private set; }
        public Medicine Medicine { get; private set; }
        private StockAdjustment() { }
        internal StockAdjustment(int medicineId, int quantity, string reason, int pharmacistId)
        {
            if (quantity == 0)
                throw new InvalidMedicineQuantityException(); 

            if (string.IsNullOrWhiteSpace(reason))
                throw new MissingAdjustmentReasonException();


            MedicineId = medicineId;
            Quantity = quantity;
            Reason = reason;
            PharmacistId = pharmacistId;
            Date = DateTime.UtcNow;
        }
    }
}
