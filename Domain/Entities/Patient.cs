using Domain.Nurses;

namespace Domain.Entities
{
    public class Patient : CommonData
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string? City { get; set; }
        public string? PhoneNumber { get; set; }
        public int? NurseId { get; set; }
        public Nurse? Nurse { get; set; }

        private readonly List<VitalSign> _vitalSigns = new();
        public IReadOnlyCollection<VitalSign> VitalSigns => _vitalSigns.AsReadOnly();
        public virtual ICollection<MedicalRecord>? MedicalRecords { get; set; } = new List<MedicalRecord>();
        public virtual ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<Prescription>? Prescriptions { get; set; } = new List<Prescription>();
        public virtual ICollection<Dispense> Dispenses { get; private set; } = new List<Dispense>();


        public VitalSign RecordVitalSigns(
            int nurseId,
            decimal temperature,
            int heartRate,
            int systolicBloodPressure,
            int diastolicBloodPressure,
            int respiratoryRate,
            int oxygenSaturation)
        {

            var vitalSign = VitalSign.Create(
                this.Id,
                nurseId,
                temperature,
                heartRate,
                systolicBloodPressure,
                diastolicBloodPressure,
                respiratoryRate,
                oxygenSaturation);

            _vitalSigns.Add(vitalSign);

            return vitalSign;
        }

    }
}
