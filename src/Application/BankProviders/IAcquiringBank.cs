using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Application.BankProviders;
public interface IAcquiringBank
{
    Task<BankPaymentResult> Process(Payment payment);
}
