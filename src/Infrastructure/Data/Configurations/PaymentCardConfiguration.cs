using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Infrastructure.Data.Configurations;
public class PaymentCardConfiguration : IEntityTypeConfiguration<PaymentCard>
{
    public void Configure(EntityTypeBuilder<PaymentCard> builder)
    {
        builder.ToTable("PaymentCards");

        builder.HasKey(x => x.Number);
    }
}
