namespace Domain.Doctors.ValueObjects;

public sealed record DoctorOffice
{
    public string Address { get; }
    public string City { get; }

    private DoctorOffice(string address, string city)
    {
        Address = address;
        City = city;
    }

    public static DoctorOffice Create(string address, string city)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new Exceptions.InvalidDoctorOfficeException("Office address is required.");
        if (string.IsNullOrWhiteSpace(city))
            throw new Exceptions.InvalidDoctorOfficeException("Office city is required.");
        var normalizedAddress = address.Trim();
        var normalizedCity = city.Trim();
        if (normalizedAddress.Length > 250)
            throw new Exceptions.InvalidDoctorOfficeException("Office address cannot exceed 250 characters.");
        if (normalizedCity.Length > 50)
            throw new Exceptions.InvalidDoctorOfficeException("Office city cannot exceed 50 characters.");
        return new DoctorOffice(normalizedAddress, normalizedCity);
    }
}
