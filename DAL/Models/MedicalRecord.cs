using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class MedicalRecord : CommonData
    {
        public int Id { get; set; }
        public required int? PatientId { get; set; }
        public required int? DoctorId { get; set; }
        public string? Diagnosis { get; set; }

        public virtual Patient? Patient { get; set; } = null!;
        public virtual Doctor? Doctor { get; set; } = null!;
        public virtual ICollection<Prescription>? Prescriptions { get; set; } = new List<Prescription>();
    }

}
