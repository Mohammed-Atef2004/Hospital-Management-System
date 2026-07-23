using MediatR;

namespace Application.Features.Nurses.Commands.RecordVitalSigns
{
    public record RecordVitalSignsCommand(
   int NurseId,
   int PatientId,
   decimal Temperature,
   int HeartRate,
   int SystolicBloodPressure,
   int DiastolicBloodPressure,
   int RespiratoryRate,
   int OxygenSaturation
) : IRequest<int>;
}
