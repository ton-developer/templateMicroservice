using Application.Abstractions;
using Domain.Entities.Users;
using Domain.Entities.Users.ValueObjects;
using Domain.Primitives;

namespace Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(AggregateId Id) : IQuery<User?>;