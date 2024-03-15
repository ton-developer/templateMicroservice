using Domain.Entities.Primitives;
using Infrastructure.Driven.Outbox;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace Infrastructure.Driven.Interceptor;

public class AggregateRootEventInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;
        
        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        
        var outBoxMessages = dbContext.ChangeTracker.Entries<AggregateRoot>()
            .Select(x => new 
            {
                x.Entity,
                Events = x.Entity.GetDomainEvents(),
                AggregateName = x.Entity.GetType().Name
            })
            .SelectMany(x =>
            {
                x.Entity.ClearDomainEvents();
                return x.Events.Select(domainEvent => new OutboxMessage(
                    Guid.NewGuid(),
                    x.AggregateName,
                    domainEvent.GetType().Name,
                    JsonConvert.SerializeObject(domainEvent),
                    DateTime.UtcNow,
                    null
                ));
            })
            .ToList();
        
        dbContext.Set<OutboxMessage>().AddRange(outBoxMessages);
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}