using Application.Features.Doctors.DTOs;
using Application.Features.Medicines.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medicines.Queries.GetAll
{
    public record GetAllMedicinesQuery() : IRequest<List<MedicineDTO>>;
}
