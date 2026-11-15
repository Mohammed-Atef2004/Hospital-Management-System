namespace Application.Common;

/// <summary>Application-facing transaction boundary implemented by the persistence layer.</summary>
public interface ITransactionManager
{
    Task<IApplicationTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

public interface IApplicationTransaction : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
}

public interface ICommandRequest<TResponse> : MediatR.IRequest<TResponse> { }
