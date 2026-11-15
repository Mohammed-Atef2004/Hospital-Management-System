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
            throw new ArgumentException("Office address is required.", nameof(address));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("Office city is required.", nameof(city));
        return new DoctorOffice(address.Trim(), city.Trim());
    }
}
