using System;

namespace DAL.Models
{
    public class Prescription : CommonData
    {
        public int Id { get; set; }


        public  int MedicalRecordId { get; set; }
        public  int DoctorId { get; set; }
        public  int PatientId { get; set; }

        public  string MedicineName { get; set; }
        public  string Dosage { get; set; }            
        public  string Frequency { get; set; }        
        public  string Duration { get; set; }         
        public string? Instructions { get; set; }             

        public DateTime IssuedDate { get; set; } = DateTime.UtcNow;

        public virtual MedicalRecord MedicalRecord { get; set; } 
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; } 
    }

  
}
