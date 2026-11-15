
[Serializable]
internal class AppointmentNotFoundException : Exception
{
    private int id;

    public AppointmentNotFoundException()
    {
    }

    public AppointmentNotFoundException(int id)
    {
        this.id = id;
    }

    public AppointmentNotFoundException(string? message) : base(message)
    {
    }

    public AppointmentNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}