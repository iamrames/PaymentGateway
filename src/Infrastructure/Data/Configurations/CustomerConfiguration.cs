using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Payments)
            .WithOne(x => x.Customer)
            .HasForeignKey(p => p.CustomerId);
    }
}
