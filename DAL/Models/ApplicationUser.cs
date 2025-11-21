using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? ProfileImageUrl { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        //Profiles
        public Doctor? DoctorProfile { get; set; }
        public Nurse? NurseProfile { get; set; }
        public Patient? PatientProfile { get; set; }

    }
}
