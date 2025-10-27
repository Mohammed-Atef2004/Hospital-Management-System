using DAL.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Patient:CommonData
    {
        public int Id { get; set; }
        public required string ApplicationUserId { get; set; }
        public  ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
