using Application.Features.Shifts.DTOs;
using AutoMapper;
using Domain.Shifts;

namespace Application.Features.Shifts.Mapping
{
    public class ShiftProfile : Profile
    {
        public ShiftProfile()
        {
            CreateMap<Shift, ShiftDTO>()
                .ConstructUsing(src => new ShiftDTO(
                    src.StartDateTime,
                    src.EndDateTime
                ));
        }
    }
}
