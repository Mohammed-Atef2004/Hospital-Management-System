using Domain.Nurses;
using Domain.SharedKernel;
using Domain.Users;

namespace Domain.Entities;

public class Patient : CommonData
{
    public int Id { get; set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public string? City { get; private set; }
    public string? PhoneNumber { get; private set; }
    public Guid? UserId { get; private set; }
    public User? Account { get; private set; }
    public int? NurseId { get; set; }
    public Nurse? Nurse { get; set; }

    private readonly List<VitalSign> _vitalSigns = new();
    public IReadOnlyCollection<VitalSign> VitalSigns => _vitalSigns.AsReadOnly();
    public virtual ICollection<MedicalRecord>? MedicalRecords { get; set; } = new List<MedicalRecord>();
    public virtual ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();
    public virtual ICollection<Prescription>? Prescriptions { get; set; } = new List<Prescription>();
    public virtual ICollection<Dispense> Dispenses { get; private set; } = new List<Dispense>();

    private Patient() { }

    public static Patient RegisterProfile(
        Guid userId, string firstName, string lastName, string address, string? city, string? phoneNumber)
    {
        if (userId == Guid.Empty) throw new DomainException("A valid account is required for the patient profile.");
        var patient = new Patient { UserId = userId };
        patient.UpdateProfile(firstName, lastName, address, city, phoneNumber);
        return patient;
    }

    public void UpdateProfile(string firstName, string lastName, string address, string? city, string? phoneNumber)
    {
        FirstName = RequiredText(firstName, nameof(firstName), 100);
        LastName = RequiredText(lastName, nameof(lastName), 100);
        Address = RequiredText(address, nameof(address), 250);
        City = OptionalText(city, nameof(city), 100);
        PhoneNumber = OptionalText(phoneNumber, nameof(phoneNumber), 20);
        UpdatedAt = DateTime.UtcNow;
    }

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
            this.Id, nurseId, temperature, heartRate, systolicBloodPressure,
            diastolicBloodPressure, respiratoryRate, oxygenSaturation);
        _vitalSigns.Add(vitalSign);
        return vitalSign;
    }

    private static string RequiredText(string value, string name, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Trim().Length > maxLength)
            throw new DomainException($"{name} must contain 1 to {maxLength} characters.");
        return value.Trim();
    }

    private static string? OptionalText(string? value, string name, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        if (value.Trim().Length > maxLength)
            throw new DomainException($"{name} cannot exceed {maxLength} characters.");
        return value.Trim();
    }
}