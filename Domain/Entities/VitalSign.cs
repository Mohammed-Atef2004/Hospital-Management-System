namespace Domain.Entities
{
    public class VitalSign
    {
        public int Id { get; private set; }

        public int PatientId { get; private set; }

        public int RecordedByNurseId { get; private set; }

        public decimal Temperature { get; private set; }

        public int HeartRate { get; private set; }

        public int SystolicBloodPressure { get; private set; }

        public int DiastolicBloodPressure { get; private set; }

        public int RespiratoryRate { get; private set; }

        public int OxygenSaturation { get; private set; }

        public DateTime RecordedAt { get; private set; }

        private VitalSign() { }

        private VitalSign(int patientId, int recordedByNurseId, decimal temperature, int heartRate, int systolicBloodPressure, int diastolicBloodPressure, int respiratoryRate, int oxygenSaturation)
        {
            PatientId = patientId;
            RecordedByNurseId = recordedByNurseId;
            Temperature = temperature;
            HeartRate = heartRate;
            SystolicBloodPressure = systolicBloodPressure;
            DiastolicBloodPressure = diastolicBloodPressure;
            RespiratoryRate = respiratoryRate;
            OxygenSaturation = oxygenSaturation;
            RecordedAt = DateTime.UtcNow;
        }

        public static VitalSign Create(
            int patientId,
            int recordedByNurseId,
            decimal temperature,
            int heartRate,
            int systolicBloodPressure,
            int diastolicBloodPressure,
            int respiratoryRate,
            int oxygenSaturation)
        {
            return new VitalSign(
                patientId,
                recordedByNurseId,
                temperature,
                heartRate,
                systolicBloodPressure,
                diastolicBloodPressure,
                respiratoryRate,
                oxygenSaturation);
        }
    }
}
