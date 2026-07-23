namespace Domain.Interfaces.Services
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        public bool IsInRole(string role);
    }
}
