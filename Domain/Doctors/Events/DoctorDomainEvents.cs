using Domain.SharedKernel;

namespace Domain.Doctors.Events;

public sealed record DoctorRegisteredDomainEvent(string FirstName, string LastName, string Specialization) : DomainEvent;
public sealed record DoctorNameCorrectedDomainEvent(int DoctorId, string PreviousName, string NewName) : DomainEvent;
public sealed record DoctorOfficeRelocatedDomainEvent(int DoctorId, string Address, string City) : DomainEvent;
public sealed record DoctorContactDetailsUpdatedDomainEvent(int DoctorId, string? PhoneNumber) : DomainEvent;
public sealed record DoctorSpecializationChangedDomainEvent(int DoctorId, string PreviousSpecialization, string NewSpecialization) : DomainEvent;
public sealed record DoctorTransferredToDepartmentDomainEvent(int DoctorId, int? PreviousDepartmentId, int? NewDepartmentId) : DomainEvent;
public sealed record DoctorDeactivatedDomainEvent(int DoctorId) : DomainEvent;
public sealed record DoctorReactivatedDomainEvent(int DoctorId) : DomainEvent;
