namespace Domain.Entities
{
    public class Nurse : Staff
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string PhoneNumber { get; private set; }
        public string? ApplicationUserId { get; private set; }
        public ApplicationUser? ApplicationUser { get; private set; }
        public int? DepartmentId { get; private set; }
        public Department? Department { get; private set; }
        public virtual ICollection<Patient>? Patients { get; private set; }


        private Nurse() { }

        private Nurse(string firstName, string lastName, string address, string city, string? phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            City = city;
            PhoneNumber = phoneNumber;

        }

        public static Nurse Register(string firstName, string lastName, string address, string city, string? phoneNumber)
        {
            return new Nurse(firstName, lastName, address, city, phoneNumber);
        }

        public void Update(string? address, string? city, string? phoneNumber, int? deparId)
        {
            Address = address;
            City = city;
            PhoneNumber = phoneNumber;
            DepartmentId = deparId;
            UpdatedAt = DateTime.UtcNow;

        }
    }
}
