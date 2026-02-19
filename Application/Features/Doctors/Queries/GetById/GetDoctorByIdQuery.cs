using Application.Features.Doctors.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Queries.GetById
{
    public record GetDoctorByIdQuery(
        int Id
    ):IRequest<DoctorDto>;
}
