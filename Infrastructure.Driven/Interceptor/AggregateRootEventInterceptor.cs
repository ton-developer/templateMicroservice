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

        string aggregateName = string.Empty;
        var outBoxMessages = dbContext.ChangeTracker.Entries<AggregateRoot<UserId>>()
            .Select(x => x.Entity)
            .SelectMany(x =>
            {
                var events = x.GetDomainEvents();
                x.ClearDomainEvents();
                aggregateName = x.GetType().Name;
                return events;
            })
            .Select(domainEvent => new OutboxMessage(
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
                        "myServiceName."+aggregateName
                    )
            ).ToList();
        
        dbContext.Set<OutboxMessage>().AddRange(outBoxMessages);
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}