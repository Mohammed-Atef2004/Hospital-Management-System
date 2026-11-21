using AutoMapper;
using Domain.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Mappings
{
    public class DoctorProfile:Profile
    {
            public DoctorProfile()
            {
                CreateMap<Doctor, DTOs.DoctorDto>()
                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                    .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null))
                    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => !src.IsDeleted));
        }
    }
}
