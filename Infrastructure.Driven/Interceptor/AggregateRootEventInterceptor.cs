using System.Text;
using System.Text.Json;
using Domain.Entities.Primitives;
using Domain.Entities.Users.ValueObjects;
using Domain.Primitives;
using Infrastructure.Driven.Outbox;
using Microsoft.EntityFrameworkCore;
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
        
        var outBoxMessages = dbContext.ChangeTracker.Entries<AggregateRoot<UserId>>()
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
                    domainEvent.GetType().Name,
                    JsonConvert.SerializeObject(
                        domainEvent,
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        }
                    ),
                    DateTime.UtcNow,
                    null,
                    new StringBuilder("myServiceName.").Append(x.AggregateName).ToString()
                ));
            })
            .ToList();
        
        dbContext.Set<OutboxMessage>().AddRange(outBoxMessages);
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}