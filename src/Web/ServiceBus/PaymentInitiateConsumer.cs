using MassTransit;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Application.BankProviders;
using PaymentGateway.Domain.Contracts;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Web.ServiceBus;

public class PaymentInitiateConsumer(IRepository<Payment> repository, 
    IAcquiringBank acquiringBank,
    IUnitOfWork uow) : IConsumer<PaymentInitiateMessage>
{

    public async Task Consume(ConsumeContext<PaymentInitiateMessage> context)
    {
        var message = context.Message;
        var payment = await repository.Query
            .Include(x => x.Card)
            .Include(x => x.Customer)
            .AsNoTracking()
            .FirstAsync(x => x.Id == message.Id);

        var bankProcessResult = await acquiringBank.Process(payment);

        if (bankProcessResult.Success)
            payment.Processed(bankProcessResult.Id!);
        else
            payment.Rejected(bankProcessResult.Message!);

        await uow.SaveChangesAsync();
    }
}
