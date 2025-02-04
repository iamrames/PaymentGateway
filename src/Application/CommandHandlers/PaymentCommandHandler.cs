using MassTransit;
using MediatR;
using PaymentGateway.Application.Commands;
using PaymentGateway.Domain.Contracts;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Application.CommandHandlers;
public class PaymentCommandHandler(IRepository<Payment> repository,
    IUnitOfWork uow,
    IBus bus) : IRequestHandler<PaymentCommand, Payment>
{
    public async Task<Payment> Handle(PaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = Payment.Create(request.Amount, request.Currency,
            new PaymentCard(request.Card.Type, request.Card.Name, request.Card.Number, request.Card.ExpireMonth,
                request.Card.ExpireYear, request.Card.CVV),
            new Customer(request.CustomerDetail.Name, request.CustomerDetail.Email));

        repository.Add(payment);
        await uow.SaveChangesAsync();

        await bus.Send(new PaymentInitiateMessage {  Id = payment.Id });

        return payment;
    }
}
