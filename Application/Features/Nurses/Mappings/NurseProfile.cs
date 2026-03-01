using Application.Features.Nurses.DTOs;
using AutoMapper;
using Domain.Entities;


namespace Application.Features.Nurses.Mappings
{
    public class NurseProfile : Profile
    {
        public NurseProfile()
        {
            CreateMap<Nurse, NurseDTO>()
                .ConstructUsing(src => new NurseDTO(
                    src.FirstName,
                    src.LastName,
                    src.Address,
                    src.City,
                    src.PhoneNumber,
                    src.DepartmentId
             ));
            CreateMap<Schedule, ShiftDTO>()
                .ConstructUsing(src => new ShiftDTO(
                    src.DayOfWeek,
                    src.StartTime,
                    src.EndTime
                ));
            CreateMap<Patient, PatientDTO>()
                .ConstructUsing(src => new PatientDTO(
                    src.Id,
                    src.FirstName,
                    src.LastName,
                    src.Address,
                    src.PhoneNumber
                ));
        }
    }
}
