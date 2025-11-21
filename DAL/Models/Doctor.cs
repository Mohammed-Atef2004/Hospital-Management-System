using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Doctor:CommonData
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ?PhoneNumber { get; set; }
        public string Specialization { get; set; }

        public required string? ApplicationUserId { get; set; }
        public  ApplicationUser? ApplicationUser { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public virtual ICollection<Schedule>? Schedules { get; set; }                        
    }
}
