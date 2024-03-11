using Domain.Primitives;

namespace Domain.Entities.Primitives;

public abstract class AggregateRoot<TId> where TId : AggregateId
{
    private readonly List<IDomainEvent> _domainEvents = new();
    protected AggregateRoot(TId id)
    {
        Id = id;
    }

    public TId Id { get; init; }
    
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    
    public void ClearDomainEvents() => _domainEvents.Clear();
    protected void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}