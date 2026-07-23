using Domain.Interfaces.Services;

namespace Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private static readonly int DefaultDeveloperUserId = 1;
        public int? UserId => DefaultDeveloperUserId;

        public bool IsInRole(string role)
        {
            if (UserId == DefaultDeveloperUserId)
            {
                return true;
            }

            return false;
        }
    }
}
