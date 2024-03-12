using Domain.Entities.Users.ValueObjects;
using Domain.Primitives;

namespace Domain.Entities.Users.Events;

public sealed record UserCreatedDomainEvent(AggregateId Id, Name Name, Email Email) : IDomainEvent;