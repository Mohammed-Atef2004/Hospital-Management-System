using Domain.Doctors;
using Domain.Entities;
using Domain.Departments;
using Domain.Nurses;
using Domain.Departments.Exceptions;
using Domain.Departments.Events;
using Domain.Departments.ValueObjects;
using Domain.SharedKernel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Departments
{
    public class Department : CommonData, IAggregateRoot
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public bool IsActive { get; private set; } = true;

        public virtual ICollection<Doctor> Doctors { get; private set; } = new List<Doctor>();
        public virtual ICollection<Nurse> Nurses { get; private set; } = new List<Nurse>();
        private readonly List<DomainEvent> _domainEvents = new();
        [NotMapped]
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        private Department() { } // EF Core

        // Factory Method (Task-Based API friendly)
        public static Department Register(string name, string description)
        {
            var department = new Department
            {
                Name = DepartmentName.Create(name).Value,
                Description = DepartmentDescription.Create(description).Value
            };
            department.AddDomainEvent(new DepartmentRegisteredDomainEvent(department.Name));
            return department;
        }

        public void UpdateInfo(string name, string description)
        {
            var previousName = Name;
            Name = DepartmentName.Create(name).Value;
            Description = DepartmentDescription.Create(description).Value;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new DepartmentInformationUpdatedDomainEvent(Id, previousName, Name));
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new DepartmentAlreadyInactiveException(Id);

            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new DepartmentDeactivatedDomainEvent(Id));
        }


        public void Activate()
        {
            if(IsActive)
                throw new DepartmentAlreadyActiveException(Id);
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new DepartmentActivatedDomainEvent(Id));
        }
        public void Rename(string newName)
        {
            var previousName = Name;
            Name = DepartmentName.Create(newName).Value;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new DepartmentRenamedDomainEvent(Id, previousName, Name));
        }
        public void ChangeDescription(string newDescription)
        {
            var previousDescription = Description;
            Description = DepartmentDescription.Create(newDescription).Value;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new DepartmentDescriptionChangedDomainEvent(Id, previousDescription, Description));
        }

        private void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
