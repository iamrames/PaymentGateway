using Microsoft.EntityFrameworkCore.Storage;

namespace PaymentGateway.Domain.Interfaces;
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken = default);
}

