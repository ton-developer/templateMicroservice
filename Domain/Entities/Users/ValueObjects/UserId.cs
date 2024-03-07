using Domain.Primitives;

namespace Domain.Entities.Users.ValueObjects;

public record UserId(Guid Id) : AggregateId(Id);