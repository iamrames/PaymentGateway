using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Infrastructure.Data;

namespace PaymentGateway.Infrastructure.Repositories;
public class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMediator _mediator;

    public DomainEventsDispatcher(
        ApplicationDbContext dbContext,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task DispatchEventsAsync(CancellationToken cancellationToken)
    {
        var domainEntities = _dbContext.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Count > 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents.TakeWhile(_ => !cancellationToken.IsCancellationRequested))
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}

