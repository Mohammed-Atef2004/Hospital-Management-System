using Domain.SharedKernel;

namespace Domain.Departments.Events;

public sealed record DepartmentRegisteredDomainEvent(string Name) : DomainEvent;
public sealed record DepartmentInformationUpdatedDomainEvent(int DepartmentId, string PreviousName, string NewName) : DomainEvent;
public sealed record DepartmentRenamedDomainEvent(int DepartmentId, string PreviousName, string NewName) : DomainEvent;
public sealed record DepartmentDescriptionChangedDomainEvent(int DepartmentId, string PreviousDescription, string NewDescription) : DomainEvent;
public sealed record DepartmentDeactivatedDomainEvent(int DepartmentId) : DomainEvent;
public sealed record DepartmentActivatedDomainEvent(int DepartmentId) : DomainEvent;
