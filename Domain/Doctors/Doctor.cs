using Domain.Departments;
using Domain.Doctors.Exceptions;
using Domain.Entities;
using Domain.personnel;
using Domain.SharedKernel;
using Domain.Doctors.Events;
using Domain.Doctors.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Doctors
{

    public class Doctor : Staff, IAggregateRoot
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string Specialization { get; private set; }

        private readonly List<DomainEvent> _domainEvents = new();
        [NotMapped]
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();


        public int? DepartmentId { get; private set; }
        public Department? Department { get; private set; }

        public virtual ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();


        // EF Core constructor
        private Doctor() { }

        private Doctor(
            string firstName,
            string lastName,
            string address,
            string city,
            string specialization
            )
        {
            var name = DoctorName.Create(firstName, lastName);
            var office = DoctorOffice.Create(address, city);
            var specialty = DoctorSpecialization.Create(specialization);
            FirstName = name.FirstName;
            LastName = name.LastName;
            Address = office.Address;
            City = office.City;
            Specialization = specialty.Value;
        }

        // Factory method
        public static Doctor Create(
            string firstName,
            string lastName,
            string address,
            string city,
            string specialization
           )
        {
            var doctor = new Doctor(firstName, lastName, address, city, specialization);
            doctor.AddDomainEvent(new DoctorRegisteredDomainEvent(doctor.FirstName, doctor.LastName, doctor.Specialization));
            return doctor;
        }

        // --- Task-Based Methods (Actions) ---

        public void RelocateDoctorOffice(string address, string city)
        {
            EnsureActive();
            var office = DoctorOffice.Create(address, city);
            Address = office.Address;
            City = office.City;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new DoctorOfficeRelocatedDomainEvent(Id, Address, City));
        }

        public void UpdateContactDetails(string? phoneNumber)
        {
            EnsureActive();
            if (phoneNumber?.Trim().Length > 20)
                throw new DomainException("Doctor phone number cannot exceed 20 characters.");
            PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim();
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new DoctorContactDetailsUpdatedDomainEvent(Id, PhoneNumber));
        }

        public void SetSpecialization(string specialization)
        {
            UpdateSpecialization(specialization);
        }
        public void UpdateSpecialization(string specialization)
        {
            EnsureActive();
            var previousSpecialization = Specialization;
            Specialization = DoctorSpecialization.Create(specialization).Value;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new DoctorSpecializationChangedDomainEvent(Id, previousSpecialization, Specialization));
        }

        public void CorrectName(string firstName, string lastName)
        {
            EnsureActive();
            var previousName = FullName;
            var name = DoctorName.Create(firstName, lastName);
            FirstName = name.FirstName;
            LastName = name.LastName;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new DoctorNameCorrectedDomainEvent(Id, previousName, FullName));
        }


        public void SetDepartment(Department department)
        {
            TransferToDepartment(department);
        }
        public void RemoveDepartment()
        {
            EnsureActive();
            var previousDepartmentId = DepartmentId;
            Department = null;
            DepartmentId = null;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new DoctorTransferredToDepartmentDomainEvent(Id, previousDepartmentId, null));
        }
        public void TransferToDepartment(Department newDepartment)
        {
            EnsureActive();
            ArgumentNullException.ThrowIfNull(newDepartment);
            if (!newDepartment.IsActive)
                throw new DomainException("A doctor cannot be assigned to an inactive department.");
            var previousDepartmentId = DepartmentId;
            Department = newDepartment;
            DepartmentId = newDepartment.Id;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new DoctorTransferredToDepartmentDomainEvent(Id, previousDepartmentId, DepartmentId));
        }

        //public void GrantSystemAccess(string applicationUserId)
        //{
        //    ApplicationUserId = applicationUserId;
        //}
        //public void RevokeSystemAccess()
        //{
        //    ApplicationUserId = null;
        //    ApplicationUser = null;
        //}
        //public void SetApplicationUser(ApplicationUser applicationUser)
        //{
        //    ApplicationUser = applicationUser;
        //    ApplicationUserId = applicationUser.Id;
        //}

        //public void RemoveApplicationUser()
        //{
        //    ApplicationUser = null;
        //    ApplicationUserId = null;
        //}

        // Schedule management
        //public void AddSchedule(Schedule schedule)
        //{
        //    if (Schedules.Any(s => s.DayOfWeek == schedule.DayOfWeek && s.StartTime == schedule.StartTime))
        //        throw new InvalidOperationException("Schedule already exists for this time.");

        //    Schedules.Add(schedule);
        //}

        //public void RemoveSchedule(Schedule schedule)
        //{
        //    Schedules.Remove(schedule);
        //}

        // Appointment management
        public void BookAppointment(Appointment appointment, DateTime currentTime)
        {
            ArgumentNullException.ThrowIfNull(appointment);
            if (appointment.DoctorId != Id)
                throw new DomainException("The appointment belongs to a different doctor.");
            if (appointment.AppointmentDate < currentTime)
                throw new DomainException("Cannot book an appointment in the past.");

            if (!IsAvailableForAppointment(appointment.AppointmentDate))
                throw new DoctorNotAvailableException(appointment.AppointmentDate);

            Appointments.Add(appointment);
            UpdatedAt = currentTime;
        }

        public void CancelAppointment(Appointment appointment, DateTime updatedAt)
        {
            Appointments.Remove(appointment);
            UpdatedAt = updatedAt;
        }

        // Soft delete
        public void SoftDeleteDoctor(DateTime deletedAt, string? deletedBy = null)
        {
            if (IsDeleted) throw new DomainException("Doctor is already inactive.");
            SoftDelete(deletedBy);
            DeletedAt = deletedAt;
            UpdatedAt = deletedAt;
            AddDomainEvent(new DoctorDeactivatedDomainEvent(Id));
        }

        public void RestoreDoctor()
        {
            Reactivate();
        }
        public void Deactivate()
        {
            SoftDeleteDoctor(DateTime.UtcNow);
        }
        public void Reactivate()
        {
            if (!IsDeleted) throw new DomainException("Doctor is already active.");
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new DoctorReactivatedDomainEvent(Id));
        }

        // --- Helpers ---
        public string FullName => $"{FirstName} {LastName}";

        public bool IsAvailableForAppointment(DateTime dateTime)
        {
            if (IsDeleted) return false;
            var isWithinWorkingHours = Shifts.Any(s => !s.IsDeleted &&
                s.IsActive &&
                dateTime >= s.StartDateTime &&
                dateTime < s.EndDateTime);
            if (!isWithinWorkingHours) return false;


            var hasConflict = Appointments.Any(a =>
                a.AppointmentDate == dateTime && !a.IsDeleted && !a.IsCancelled);

            return !hasConflict;
        }

        private void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);

        private void EnsureActive()
        {
            if (IsDeleted)
                throw new DomainException("An inactive doctor cannot be modified.");
        }

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}

