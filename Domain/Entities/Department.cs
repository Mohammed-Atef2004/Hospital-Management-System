using Domain.Entities;

public class Department : CommonData
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public List<Doctor> Doctors { get; private set; } = new();
    public List<Nurse> Nurses { get; private set; } = new();

    private Department() { } // EF

    private Department(string name, string description)
    {
        Name = name;
        Description = description;
        IsActive = true;
    }

    public static Department Register(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Department name is required");

        return new Department(name.Trim(), description?.Trim());
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Invalid name");

        Name = name.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeDescription(string description)
    {
        Description = description?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
