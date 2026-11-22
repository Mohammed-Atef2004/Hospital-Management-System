using Application.Features.Doctors.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Queries.GetAll
{
    public record GetAllDoctorsQuery(bool IncludeInactive = false) : IRequest<List<DoctorDto>>;
}
