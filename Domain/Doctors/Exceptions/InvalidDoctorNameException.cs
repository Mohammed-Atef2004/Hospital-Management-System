namespace Domain.Doctors.Exceptions
{
    [Serializable]
    public class InvalidDoctorNameException : ArgumentException
    {
       
        public InvalidDoctorNameException() : base("First name and last name are required.")
        {
        }
       
    }
}
