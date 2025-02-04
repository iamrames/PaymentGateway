using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Infrastructure.Data.Configurations;
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Card)
            .WithMany()
            .HasForeignKey(p => p.CardNumber);

        builder.HasOne(x => x.Customer)
            .WithMany(x => x.Payments)
            .HasForeignKey(p => p.CustomerId);
    }
}
