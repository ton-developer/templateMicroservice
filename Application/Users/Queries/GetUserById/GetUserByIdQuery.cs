using Application.Abstractions;
using Domain.Entities.Users;
using Domain.Entities.Users.ValueObjects;

namespace Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(UserId Id) : IQuery<User?>;