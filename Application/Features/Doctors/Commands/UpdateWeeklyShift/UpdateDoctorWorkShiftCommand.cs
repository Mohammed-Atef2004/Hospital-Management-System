using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Doctors.Commands.UpdateWeeklyShift
{
    public record UpdateDoctorWorkShiftCommand(
    int DoctorId,
    int ScheduleId,
    DayOfWeek NewDay,
    TimeSpan NewStart,
    TimeSpan NewEnd
) : IRequest;
}
