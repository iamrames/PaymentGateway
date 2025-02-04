using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Domain.Entities;
public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Domain events occurred.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Clear domain events.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// Add domain event.
    /// </summary>
    /// <param name="domainEvent"></param>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}

