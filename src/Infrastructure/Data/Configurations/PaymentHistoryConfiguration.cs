using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Infrastructure.Data.Configurations;

public class PaymentHistoryConfiguration : IEntityTypeConfiguration<PaymentHistory>
{
    public void Configure(EntityTypeBuilder<PaymentHistory> builder)
    {
        builder.ToTable("PaymentHistories");

        builder.HasKey(x => x.Id);
    }
}
