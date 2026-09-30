using Application.Features.Appointments.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Appointments.Mappings
{
    public class AppointmentProfile:Profile
    {
       public AppointmentProfile() 
        {
            CreateMap<Appointment,AppointmentResponse>().ReverseMap();
        }
    }
}
