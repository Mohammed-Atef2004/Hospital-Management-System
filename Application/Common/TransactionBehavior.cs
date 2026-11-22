using MediatR;
using Domain.SharedKernel;

namespace Application.Common;

/// <summary>Wraps write requests in one persistence transaction. Queries never open transactions.</summary>
public sealed class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandRequest<TResponse>
{
    private readonly ITransactionManager _transactions;

    public TransactionBehavior(ITransactionManager transactions) => _transactions = transactions;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        await using var transaction = await _transactions.BeginTransactionAsync(cancellationToken);
        try
        {
            var response = await next();
            if (response is Result result && result.IsFailure && request is not ICommitTransactionOnFailure)
            {
                await transaction.RollbackAsync(cancellationToken);
                return response;
            }
            await transaction.CommitAsync(cancellationToken);
            return response;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}

/// <summary>Marks commands whose rejected result still records security state, such as failed-login counters.</summary>
public interface ICommitTransactionOnFailure { }
