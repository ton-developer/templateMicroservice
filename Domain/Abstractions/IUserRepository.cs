using Domain.Entities;
using Domain.Entities.Users;
using Domain.Entities.Users.ValueObjects;
using Domain.Primitives;

namespace Domain.Abstractions;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(AggregateId id, CancellationToken cancellationToken = default);
    Task<User> AddAsync(User user);
}