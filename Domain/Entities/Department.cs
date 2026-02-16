using Domain.Exceptions.Department;

namespace Domain.Entities
{
    public class Department : CommonData
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public bool IsActive { get; private set; } = true;

        public virtual ICollection<Doctor>? Doctors { get; private set; }
        public virtual ICollection<Nurse>? Nurses { get; private set; }

        private Department() { } // EF Core

        // Factory Method (Task-Based API friendly)
        public static Department Register(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DepartmentInvalidNameException();

            return new Department
            {
                Name = name.Trim(),
                Description = description?.Trim() ?? string.Empty
            };
        }

        public void UpdateInfo(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DepartmentInvalidNameException();

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new DepartmentAlreadyInactiveException(Id);

            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            if(IsActive)
                throw new DepartmentAlreadyActiveException(Id);
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
