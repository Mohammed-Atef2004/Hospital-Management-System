using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.DefineWeeklyShift
{
    public record DefineDoctorWeeklyShiftCommand(
     int DoctorId,
     DayOfWeek DayOfWeek,
     TimeSpan StartTime,
     TimeSpan EndTime
 ) : IRequest;
}
