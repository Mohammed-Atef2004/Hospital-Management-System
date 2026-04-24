using Domain.Exceptions.Medicine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Dispense
    {
        //public Guid Id { get; private set; }
        public int Id { get; private set; }
        public int Quantity { get; private set; }
        public int PharmacistId { get; private set; }
        public DateTime Date { get; private set; }

        public int MedicineId { get; private set; }
        public Medicine Medicine { get; private set; }
        public int PatientId { get; private set; }
        public Patient Patient { get; private set; }

        private Dispense() { }

        internal Dispense(int medicineId, int patientId, int quantity, int pharmacistId)
        {
            if (quantity <= 0)
                throw new InvalidMedicineQuantityException();

            MedicineId = medicineId;
            PatientId = patientId;
            Quantity = quantity;
            PharmacistId = pharmacistId;
            Date = DateTime.UtcNow;
        }
    }
}
