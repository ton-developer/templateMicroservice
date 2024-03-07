using Domain.Entities.Users.ValueObjects;
using Domain.Primitives;

namespace Domain.Entities.Users.Events;

public sealed record UserCreatedDomainEvent(UserId Id, Name Name, Email Email) : IDomainEvent;