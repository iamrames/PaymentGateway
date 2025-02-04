using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Infrastructure.Data;

namespace PaymentGateway.Infrastructure.Repositories;

public class EntityFrameworkUnitOfWork : IUnitOfWork
{
    private readonly IDomainEventsDispatcher _domainEventsDispatcher;
    private readonly DbContext _dbContext;

    public EntityFrameworkUnitOfWork(
        IDomainEventsDispatcher domainEventsDispatcher,
        ApplicationDbContext dbContext)
    {
        _domainEventsDispatcher = domainEventsDispatcher;
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _domainEventsDispatcher.DispatchEventsAsync(cancellationToken);
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken = default) =>
        await _dbContext.Database.BeginTransactionAsync(cancellationToken);
}
