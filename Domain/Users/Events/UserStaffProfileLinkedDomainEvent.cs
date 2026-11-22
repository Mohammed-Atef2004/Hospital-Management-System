using Domain.SharedKernel;

namespace Domain.Users.Events;

public sealed record UserStaffProfileLinkedDomainEvent(Guid UserId, int StaffId, UserRole Role) : DomainEvent;
