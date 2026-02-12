using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Department : CommonData
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public virtual ICollection<Doctor>? Doctors { get; set; }
        public virtual ICollection<Nurse>? Nurses { get; set; }
    }
}
