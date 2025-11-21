using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Appointment: CommonData
    {
        public int Id { get; set; }
        public  DateTime AppointmentDate { get; set; }
        public bool IsCancelled { get; set; } = false;

        public string Description { get; set; }
        public required int? PatientId { get; set; }
        public Patient? Patient { get; set; }
        public  int? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
    

    }
}
