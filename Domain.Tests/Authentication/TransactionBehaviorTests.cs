using Application.Common;
using Domain.SharedKernel;
using MediatR;

namespace Domain.Tests.Authentication;

public sealed class TransactionBehaviorTests
{
    [Fact]
    public async Task FailedLoginCommand_CommitsFailedAttemptState()
    {
        var manager = new RecordingTransactionManager();
        var behavior = new TransactionBehavior<FailedLoginCommand, Result>(manager);

        var result = await behavior.Handle(
            new FailedLoginCommand(),
            () => Task.FromResult(Result.Failure(new Error("Auth.InvalidCredentials", "Invalid credentials."))),
            CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(1, manager.Transaction.CommitCount);
        Assert.Equal(0, manager.Transaction.RollbackCount);
    }

    [Fact]
    public async Task FailedOrdinaryCommand_RollsBack()
    {
        var manager = new RecordingTransactionManager();
        var behavior = new TransactionBehavior<OrdinaryCommand, Result>(manager);

        await behavior.Handle(
            new OrdinaryCommand(),
            () => Task.FromResult(Result.Failure(new Error("Invalid", "Invalid."))),
            CancellationToken.None);

        Assert.Equal(0, manager.Transaction.CommitCount);
        Assert.Equal(1, manager.Transaction.RollbackCount);
    }

    private sealed record FailedLoginCommand : ICommandRequest<Result>, ICommitTransactionOnFailure;
    private sealed record OrdinaryCommand : ICommandRequest<Result>;

    private sealed class RecordingTransactionManager : ITransactionManager
    {
        public RecordingTransaction Transaction { get; } = new();
        public Task<IApplicationTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
            => Task.FromResult<IApplicationTransaction>(Transaction);
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken) => Task.FromResult(0);
    }

    private sealed class RecordingTransaction : IApplicationTransaction
    {
        public int CommitCount { get; private set; }
        public int RollbackCount { get; private set; }
        public Task CommitAsync(CancellationToken cancellationToken)
        {
            CommitCount++;
            return Task.CompletedTask;
        }
        public Task RollbackAsync(CancellationToken cancellationToken)
        {
            RollbackCount++;
            return Task.CompletedTask;
        }
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}