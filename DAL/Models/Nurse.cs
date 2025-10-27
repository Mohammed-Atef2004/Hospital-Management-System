using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Nurse: CommonData
    {
        public int Id { get; set; }
        public  string ApplicationUserId { get; set; }
        public  ApplicationUser ApplicationUser { get; set; }
        public int DepartmentId { get; set; }
        public  Department Department { get; set; }
        public virtual ICollection<Patient>? Patients { get; set; }
    }
}
