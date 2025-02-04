namespace PaymentGateway.Domain.Interfaces;
public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync(CancellationToken cancellationToken);
}
