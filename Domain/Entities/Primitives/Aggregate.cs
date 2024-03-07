using Domain.Primitives;

namespace Domain.Entities.Primitives;

public abstract class Aggregate<TId> where TId : AggregateId
{
    private readonly List<IDomainEvent> _domainEvents = new();
    protected Aggregate(TId id)
    {
        Id = id;
    }

    public TId Id { get; init; }
    
    public List<IDomainEvent> DomainEvents => _domainEvents.ToList();
    
    protected void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}