using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Application.Common.Interfaces;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Infrastructure.Identity;

namespace PaymentGateway.Infrastructure.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Payment> Payments => Set<Payment>();

    public DbSet<PaymentCard> PaymentCards => Set<PaymentCard>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<PaymentHistory> PaymentHistories => Set<PaymentHistory>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
