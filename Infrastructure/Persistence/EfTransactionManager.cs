using Application.Common;
using Infastructure.Presistence.Database;
using Microsoft.EntityFrameworkCore.Storage;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public sealed class EfTransactionManager : ITransactionManager
{
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger<EfTransactionManager> _logger;

    public EfTransactionManager(
        ApplicationDbContext context,
        IMediator mediator,
        ILogger<EfTransactionManager> logger)
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<IApplicationTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        => new EfApplicationTransaction(_context,
            await _context.Database.BeginTransactionAsync(cancellationToken), _mediator, _logger);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => _context.SaveChangesAsync(cancellationToken);

    private sealed class EfApplicationTransaction : IApplicationTransaction
    {
        private readonly IDbContextTransaction _transaction;
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private bool _completed;

        public EfApplicationTransaction(
            ApplicationDbContext context,
            IDbContextTransaction transaction,
            IMediator mediator,
            ILogger logger)
        {
            _context = context;
            _transaction = transaction;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            var aggregates = _context.ChangeTracker.Entries<Domain.SharedKernel.IAggregateRoot>()
                .Select(entry => entry.Entity)
                .Where(aggregate => aggregate.DomainEvents.Count != 0)
                .ToArray();
            var events = aggregates.SelectMany(aggregate => aggregate.DomainEvents).ToArray();

            await _transaction.CommitAsync(cancellationToken);
            _completed = true;

            foreach (var domainEvent in events)
            {
                try
                {
                    await _mediator.Publish(domainEvent, cancellationToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Domain event {EventType} failed after transaction commit.", domainEvent.GetType().Name);
                }
            }

            foreach (var aggregate in aggregates)
                aggregate.ClearDomainEvents();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            if (_completed) return;
            await _transaction.RollbackAsync(cancellationToken);
            _completed = true;
        }
        public ValueTask DisposeAsync() => _transaction.DisposeAsync();
    }
}
