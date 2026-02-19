namespace Domain.Exceptions.Doctors
{
    [Serializable]
    internal class InvalidDoctorNameException : Exception
    {
       
        public InvalidDoctorNameException() : base($"The First Name Or Second Name Shouldn't Be Empty")
        {
        }
       
    }
}