using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.DTOs
{
    public record DoctorDto(
     int Id,
     string FullName,
     string Specialization,
     string? DepartmentName,
     string City
 );
}
