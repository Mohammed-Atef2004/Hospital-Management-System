using DAL.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Pharmacy
    {
        public int Id { get; set; }
        public ICollection<Medicine>? Medicines { get; set; }
    }
}
