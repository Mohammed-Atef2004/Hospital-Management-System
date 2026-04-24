using Application.Features.Medicines.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medicines.Mappings
{
    public class MedicineProfile : Profile
    {
        public MedicineProfile()
        {
            CreateMap<Domain.Entities.Medicine, DTOs.MedicineDTO>();

            CreateMap<StockAdjustment, StockAdjustmentDTO>();

            CreateMap<Dispense, DispenseDTO>()
                .ForMember(
                    dest => dest.PatientFullName,
                    opt => opt.MapFrom(src => $"{src.Patient.FirstName} {src.Patient.LastName}")
                );

            CreateMap<Medicine, MedicineDetailsDTO>()
                .ForMember(dest => dest.StockAdjustments, opt => opt.MapFrom(src => src.StockAdjustments))
                .ForMember(dest => dest.Dispenses, opt => opt.MapFrom(src => src.Dispenses));
        }
    }
}
