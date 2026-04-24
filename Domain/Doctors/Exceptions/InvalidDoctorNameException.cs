namespace Domain.Doctors.Exceptions
{
    [Serializable]
    internal class InvalidDoctorNameException : Exception
    {
       
        public InvalidDoctorNameException() : base($"The First Name Or Second Name Shouldn't Be Empty")
        {
        }
       
    }
}