using Domain.Exceptions.Doctors;

namespace Domain.Entities
{

    public class Doctor : Staff
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string Specialization { get; private set; }

        public string? ApplicationUserId { get; private set; }
        public ApplicationUser? ApplicationUser { get; private set; }

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
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required.");
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required.");

            FirstName = firstName;
            LastName = lastName;
            Address = address;
            City = city;
            Specialization = specialization;
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
            return new Doctor(firstName, lastName, address, city, specialization);
        }

        // --- Task-Based Methods (Actions) ---

        public void RelocateDoctorOffice(string address, string city)
        {
            Address = address;
            City = city;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateContactDetails(string? phoneNumber)
        {
            PhoneNumber = phoneNumber;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetSpecialization(string specialization)
        {
            if (string.IsNullOrWhiteSpace(specialization))
                throw new InvalidSpecializationException();
            Specialization = specialization;
        }
        public void UpdateSpecialization(string specialization)
        {
            if (string.IsNullOrWhiteSpace(specialization))
                throw new InvalidSpecializationException();
            Specialization = specialization;
            UpdatedAt = DateTime.UtcNow;
        }

        public void CorrectName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            UpdatedAt = DateTime.Now;
        }


        public void SetDepartment(Department department)
        {
            Department = department;
            DepartmentId = department.Id;
        }
        public void RemoveDepartment()
        {
            Department = null;
            DepartmentId = null;
        }
        public void TransferToDepartment(Department newDepartment)
        {
            Department = newDepartment;
            DepartmentId = newDepartment.Id;
        }

        public void GrantSystemAccess(string applicationUserId)
        {
            ApplicationUserId = applicationUserId;
        }
        public void RevokeSystemAccess()
        {
            ApplicationUserId = null;
            ApplicationUser = null;
        }
        public void SetApplicationUser(ApplicationUser applicationUser)
        {
            ApplicationUser = applicationUser;
            ApplicationUserId = applicationUser.Id;
        }

        public void RemoveApplicationUser()
        {
            ApplicationUser = null;
            ApplicationUserId = null;
        }

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
            if (appointment.AppointmentDate < currentTime)
                throw new InvalidOperationException("Cannot book an appointment in the past.");

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
            SoftDelete(deletedBy);
            DeletedAt = deletedAt;
        }

        public void RestoreDoctor()
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
        }
        public void Deactivate()
        {
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
        }
        public void Reactivate()
        {
            IsDeleted = false;
            UpdatedAt = DateTime.UtcNow;
        }

        // --- Helpers ---
        public string FullName => $"{FirstName} {LastName}";

        public bool IsAvailableForAppointment(DateTime dateTime)
        {
            var isWithinWorkingHours = Schedules.Any(s =>
                s.DayOfWeek == dateTime.DayOfWeek &&
                dateTime.TimeOfDay >= s.StartTime &&
                dateTime.TimeOfDay < s.EndTime);

            if (!isWithinWorkingHours) return false;


            var hasConflict = Appointments.Any(a =>
                a.AppointmentDate == dateTime && !a.IsDeleted);

            return !hasConflict;
        }
    }
}

