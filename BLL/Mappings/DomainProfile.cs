using AutoMapper;
using BLL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappings
{
    public class DomainProfile:Profile
    {
       public DomainProfile() 
       {
            CreateMap<PatientDTO, Patient>().ReverseMap();
            CreateMap<CreatePatientDTO, Patient>().ReverseMap();
            CreateMap<DoctorDTO, Doctor>().ReverseMap();
            CreateMap<CreateDoctorDTO, Doctor>().ReverseMap();
        }
    }
}
