using MediatR;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Application.Commands;
public class PaymentCommand : IRequest<Payment>
{
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
    public required PaymentCardCommand Card { get; set; }

    public required CustomerDetailCommand CustomerDetail { get; set; }

    public class CustomerDetailCommand
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }

    public class PaymentCardCommand
    {
        public required string Type { get; set; }
        public required string Name { get; set; }
        public required string Number { get; set; }
        public int ExpireMonth { get; set; }
        public int ExpireYear { get; set; }
        public int CVV { get; set; }
    }
}
