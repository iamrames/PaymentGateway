using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<Payment> Payments { get; }

    DbSet<PaymentCard> PaymentCards { get; }
    DbSet<Customer> Customers { get; }
    DbSet<PaymentHistory> PaymentHistories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
