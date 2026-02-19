using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Departments.Mappings
{
    public class DepartmentProfile:Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Domain.Entities.Department, DTOs.DepartmentDto>().ReverseMap();   
        }
    }
}
