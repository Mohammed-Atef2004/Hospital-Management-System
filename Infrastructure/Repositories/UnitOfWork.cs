
using Domain.Departments;
using Domain.Doctors;
using Domain.Medicines;
using Domain.Nurses;
using Domain.PatientAssignments;
using Domain.personnel;
using Domain.Repositories;
using Domain.Schedules;
using Infastructure.Presistence.Database;
using Infrastructure.Repositories;
using MediatR;

namespace Infastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        public IPatientRepository Patient { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public IDoctorRepository Doctor { get; private set; }
        public IMedicineRepository Medicine { get; private set; }
        public INurseRepository Nurse { get; private set; }
        public IShiftRepository Shift { get; private set; }
        public IStaffRepository Staff { get; private set; }
        public IAssignPatientRepository PatientAssignment
        { get; private set; }

        public UnitOfWork(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
            Patient = new PatientRepository(_context);
            Department = new DepartmentRepository(_context);
            Doctor = new DoctorRepository(_context);

            Nurse = new NurseRepository(_context);
            Shift = new ShiftRepository(_context);
            Medicine = new MedicineRepository(_context);
            Staff = new StaffRepository(_context);
            PatientAssignment = new AssignPatientRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            var aggregates = _context.ChangeTracker
                .Entries<Domain.SharedKernel.IAggregateRoot>()
                .Select(entry => entry.Entity)
                .Where(entity => entity.DomainEvents.Count > 0)
                .ToArray();
            var domainEvents = aggregates.SelectMany(aggregate => aggregate.DomainEvents).ToArray();

            var result = await _context.SaveChangesAsync();
            foreach (var domainEvent in domainEvents)
                await _mediator.Publish(domainEvent);
            foreach (var aggregate in aggregates)
                aggregate.ClearDomainEvents();

            return result;

        }

        public void Dispose()
        {
            _context.Dispose();
        }


    }
}
